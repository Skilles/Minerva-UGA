import { createStyles, Container, Text, Button, Group, Paper } from '@mantine/core';
import { GithubIcon } from '@mantine/ds';
import {Link} from "react-router-dom";
import {useAuth} from "../hooks/useAuth";
import Dashboard from "./Dashboard";

const BREAKPOINT = '@media (max-width: 755px)';

const useStyles = createStyles((theme) => ({
    wrapper: {
        position: 'relative',
        boxSizing: 'border-box',
        backgroundColor: theme.colorScheme === 'dark' ? theme.colors.dark[8] : theme.colors.gray[0],
    },

    inner: {
        position: 'relative',
        paddingTop: 200,
        paddingBottom: 120,

        [BREAKPOINT]: {
            paddingBottom: 80,
            paddingTop: 80,
        },
    },

    title: {
        fontFamily: `Greycliff CF, ${theme.fontFamily}`,
        fontSize: 62,
        fontWeight: 900,
        lineHeight: 1.1,
        margin: 0,
        padding: 0,
        color: theme.colorScheme === 'dark' ? theme.white : theme.black,

        [BREAKPOINT]: {
            fontSize: 42,
            lineHeight: 1.2,
        },
    },

    description: {
        marginTop: theme.spacing.xl,
        fontSize: 24,

        [BREAKPOINT]: {
            fontSize: 18,
        },
    },

    controls: {
        marginTop: theme.spacing.xl * 2,

        [BREAKPOINT]: {
            marginTop: theme.spacing.xl,
        },
    },

    control: {
        height: 54,
        paddingLeft: 38,
        paddingRight: 38,

        [BREAKPOINT]: {
            height: 54,
            paddingLeft: 18,
            paddingRight: 18,
            flex: 1,
        },
    },
}));

export default function IntroSection() {
    const { classes } = useStyles();

    return (
        <div className={classes.wrapper}>
            <Container size={800} className={classes.inner} style={{
                height: '100vh',
            }}>
                <h1 className={classes.title}>
                    Welcome to{' '}
                    <Text component="span" variant="gradient" gradient={{ from: 'blue', to: 'cyan' }} inherit>
                        Minerva
                    </Text>{' '}
                </h1>

                <Text className={classes.description} color="dimmed">
                    Discover classes and professors at the University of Georgia and plan your semester - Minerva gives you many tools to craft a perfect schedule.
                </Text>

                <Group className={classes.controls}>
                    <Link to="/register">
                        <Button
                            size="xl"
                            className={classes.control}
                            variant="gradient"
                            gradient={{ from: 'blue', to: 'cyan' }}
                        >
                            Register
                        </Button>
                    </Link>
                    <Link to="/login">
                        <Button
                            component="a"
                            size="xl"
                            variant="default"
                            className={classes.control}
                        >
                            Login
                        </Button>
                    </Link>
                </Group>
            </Container>
        </div>
    );
}