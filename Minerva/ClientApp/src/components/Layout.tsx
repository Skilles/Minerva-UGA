import React, {useState} from 'react';
import {AppShell, ColorSchemeProvider, ColorScheme, MantineProvider} from "@mantine/core";
import NavbarColored from "./NavBar";
import {useColorScheme} from "@mantine/hooks";
import {useAuth} from "../hooks/useAuth";

export default function Layout(props: any) {
    const preferredColorScheme = useColorScheme();
    const [colorScheme, setColorScheme] = useState<ColorScheme>(preferredColorScheme);
    const toggleColorScheme = (value?: ColorScheme) => setColorScheme(value || (colorScheme === 'dark' ? 'light' : 'dark'));
    
    const { isLoggedIn } = useAuth();
    
    return (
        <ColorSchemeProvider colorScheme={colorScheme} toggleColorScheme={toggleColorScheme}>
            <MantineProvider theme={{ colorScheme }} withGlobalStyles withNormalizeCSS>
                <AppShell
                    padding="sm"
                    layout="alt"
                    fixed={false}
                    navbar={<NavbarColored />}
                    hidden={!isLoggedIn}
                    zIndex={5}
                    styles={(theme) => ({
                        main: {
                            backgroundColor:
                                theme.colorScheme === 'dark' ? theme.colors.dark[8] : theme.colors.gray[0],
                        },
                    })}
                >
                    {props.children}
                </AppShell>
            </MantineProvider>
        </ColorSchemeProvider>
    );
}
