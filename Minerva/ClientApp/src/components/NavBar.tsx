import React, {useEffect, useState} from 'react';
import {
    Box,
    Navbar,
    Tooltip,
    UnstyledButton,
    createStyles,
    Stack,
    useMantineColorScheme, MantineTheme, Group
} from '@mantine/core';
import { MantineLogo } from '@mantine/ds';
import {
    Home2,
    Map2,
    ListSearch,
    Calendar,
    User,
    Settings,
    Login,
    Logout, Icon, 
} from 'tabler-icons-react';
import AppRoutes from "../AppRoutes";
import {useAuth} from "../hooks/useAuth";
import {Link} from "react-router-dom";

const useStyles = createStyles((theme: MantineTheme) => ({
    link: {
        width: 50,
        height: 50,
        borderRadius: theme.radius.md,
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        color: theme.colorScheme === 'dark' ? theme.colors.dark[0] : theme.colors.gray[7],

        '&:hover': {
            backgroundColor: theme.colorScheme === 'dark' ? theme.colors.dark[5] : theme.colors.gray[1],
        },
    },

    active: {
        '&, &:hover': {
            backgroundColor: theme.fn.variant({ variant: 'light', color: theme.primaryColor }).background,
            color: theme.fn.variant({ variant: 'light', color: theme.primaryColor }).color,
        },
    },
}));

interface NavbarLinkProps {
    icon: Icon;
    label: string;
    active?: boolean;
    onClick?(): void;
    to?: string;
}

function NavbarLink({ icon: Icon, label, active, onClick, to }: NavbarLinkProps) {
    const { classes, cx } = useStyles();
    return (
        <Link to={to || '/'}>
            <Tooltip label={label} position="right" transitionDuration={0}>
                <UnstyledButton onClick={onClick} className={cx(classes.link, { [classes.active]: active })}>
                    <Icon size={28} />
                </UnstyledButton>
            </Tooltip>
        </Link>
    );
}

const icons = {
    'Home': Home2,
    'Planner': Calendar,
    'Map': Map2,
    'Search': ListSearch,
    'Profile': User,
    'Settings': Settings,
}

function Brand() {
    const { colorScheme, toggleColorScheme } = useMantineColorScheme();

    return (
        <Box
            sx={(theme: MantineTheme) => ({
                paddingLeft: theme.spacing.xs,
                paddingRight: theme.spacing.xs,
                paddingBottom: theme.spacing.lg,
                borderBottom: `1px solid ${
                    theme.colorScheme === 'dark' ? theme.colors.dark[4] : theme.colors.gray[2]
                }`,
            })}
        >
            <Group position="apart">
                <MantineLogo type="mark" inverted={colorScheme === 'dark'} size={36} onClick={() => toggleColorScheme()} />
            </Group>
        </Box>
    );
}

export default function NavbarColored() {
    const [active, setActive] = useState<string>('Home');

    const { isLoggedIn, logout } = useAuth();
    
    useEffect(() => {
        const path = window.location.pathname;
        const pathParts = path.split('/');
        let pathPart = pathParts[pathParts.length - 1];
        if (pathPart === '') {
            pathPart = 'Home';
        }
        setActive(pathPart);
    }, []);
    
    const links = AppRoutes.filter((link) => {
        if (link.navLabel === undefined) {
            return false;
        }
        
        if (link.authRequired) {
            return isLoggedIn;
        }

        return true;
    }).map((link) => (
        <NavbarLink
            icon={icons[link.navLabel as keyof typeof icons]}
            label={link.navLabel as string}
            key={link.navLabel}
            active={link.navLabel === active}
            onClick={() => setActive(link.navLabel as string)}
            to={link.path}
        />
    ));

    return (
        <Navbar
            height="100vh"
            width={{ base: 80 }}
            p="md"
            sx={(theme) => ({
                backgroundColor: theme.fn.variant({ variant: 'gradient', color: theme.colorScheme === 'dark' ? theme.colors.dark[8] : theme.white }).background,
            })}
        >
            <Navbar.Section mt="xs">
                    <Brand />
            </Navbar.Section>
            <Navbar.Section grow mt="md">
                <Stack justify="center" spacing={15}>
                    {links}
                </Stack>
            </Navbar.Section>
            <Navbar.Section>
                <Stack justify="center" spacing={15}>
                    {isLoggedIn ? <>
                            <NavbarLink icon={User} label="Profile" to="/profile" />
                            <NavbarLink icon={Logout} label="Logout" onClick={logout}/></> :
                        <NavbarLink icon={Login} label="Login" to="/login" />
                        }
                </Stack>
            </Navbar.Section>
        </Navbar>
    );
}