import React, {useEffect, useState} from 'react'

import {fetchWithAuth} from "../ApiFetch";
import {DirectionsRenderer, GoogleMap, Marker, useJsApiLoader} from '@react-google-maps/api';
import {User} from "../hooks/useUser";
import {useAuth} from "../hooks/useAuth";

let marker: google.maps.Marker;

const containerStyle = {
    width: "100%",
    height: "100%",
};

const center = {
    lat: 33.95192939144816,
    lng: -83.37592267594427
};

const buildings = [
    'Joe Brown',
    'Orkin Hall',
    'Baldwin Hall',
]

const getBuildingNames = (buildings: string[]) => {
    return buildings.map((building) => {
        return building + ',UGA';
    })
}

const getBuildingNamesFromApi = async (user: User): Promise<string[]> => {
    const response = await fetchWithAuth('map/buildings', user);
    return await response.json();
}

export function Map() {
    const {isLoaded} = useJsApiLoader({
        googleMapsApiKey: "AIzaSyBN89q8mk_8-qj0DKwiVK5-BAHDyEZqCMs",
        libraries: ['places'],
    })

    const {user} = useAuth();

    const [directionsResponse, setDirectionsResponse] = useState<google.maps.DirectionsResult | undefined>(undefined);
    //const [distance, setDistance] = React.useState<string>('')
    //const [duration, setDuration] = React.useState<string>('')

    useEffect(() => {
        if (!isLoaded) {
            return;
        }
        getBuildingNamesFromApi(user!).then((data) => {
            console.log("fetched building names")
            calculateRoute(getBuildingNames(data)).then((response) => {
                setDirectionsResponse(response);
            })
        }).catch(() => {
            console.log("failed to fetch building names")
            calculateRoute(getBuildingNames(buildings)).then((response) => {
                setDirectionsResponse(response);
            })
        })
    }, [isLoaded]);

    if (!isLoaded) {
        return <div>Loading...</div>
    }

    async function calculateRoute(buildings: string[]): Promise<google.maps.DirectionsResult> {
        const directionsService = new google.maps.DirectionsService();

        // Create a DirectionsRequest object literal
        const request: google.maps.DirectionsRequest = {
            origin: buildings[0],
            waypoints: buildings.slice(1, buildings.length - 1).map((building) => {
                return {
                    location: building,
                    stopover: true,
                }
            }),
            destination: buildings[buildings.length - 1],
            travelMode: google.maps.TravelMode.WALKING
        };

        function toggleBounce() {
            if (marker.getAnimation() !== null) {
                marker.setAnimation(null);
            } else {
                marker.setAnimation(google.maps.Animation.BOUNCE);
            }
        }

        // Call the route() method of the DirectionsService object and pass it the request and callback
        return await directionsService.route(request, function (response, status) {
            if (status === 'OK') {
                console.log(response);
                return response;
            } else {
                console.error('Directions request failed due to ' + status);
            }
        });

    }

    function clearRoute() {
        //setDirectionsResponse(null)
        //setDistance('')
        //setDuration('')
    }

    return (
        <GoogleMap
            center={center}
            zoom={16}
            mapContainerStyle={{width: '100%', height: '100%'}}
            options={{
                //zoomControl: false,
                //streetViewControl: false,
                //mapTypeControl: false,
                //fullscreenControl: false,
            }}
        >
            <Marker
                animation={google.maps.Animation.BOUNCE}
                position={center}/>
            <DirectionsRenderer directions={directionsResponse}/>
        </GoogleMap>
    )
}

export default React.memo(Map);