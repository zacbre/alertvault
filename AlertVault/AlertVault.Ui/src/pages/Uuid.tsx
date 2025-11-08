import { Container } from "@mui/material";
import { CenterContainer } from "../components/CenterContainer";
import { PaddedContainer } from "../components/PaddedContainer";
import { FetchAlert } from "../components/FetchAlert";

export const Uuid = () => {
    return (
    <CenterContainer>
        <Container maxWidth="sm">
        <PaddedContainer>
            <FetchAlert />
        </PaddedContainer>
        </Container>
    </CenterContainer>);
};