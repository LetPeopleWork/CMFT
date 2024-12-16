import React, { useContext, useEffect, useState } from "react";
import { IDataRetentionSettings } from "../../../models/AppSettings/DataRetentionSettings";
import { Container, TextField } from "@mui/material";
import Grid from '@mui/material/Grid2';
import LoadingAnimation from "../../../components/Common/LoadingAnimation/LoadingAnimation";
import ActionButton from "../../../components/Common/ActionButton/ActionButton";
import { ApiServiceContext } from "../../../services/Api/ApiServiceContext";

const DataRetentionSettingsTab: React.FC = () => {
    const [dataRetentionSettings, setDataRetentionSettings] = useState<IDataRetentionSettings | null>(null);
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [hasError, setHasError] = useState<boolean>(false);

    const { settingsService } = useContext(ApiServiceContext);

    const fetchSettings = async () => {
        setIsLoading(true);
        setHasError(false);

        try {
            const loadedSettings = await settingsService.getDataRetentionSettings();
            setDataRetentionSettings(loadedSettings);
        } catch {
            setHasError(true);
        } finally {
            setIsLoading(false);
        }
    };

    const updateSettings = async () => {
        if (dataRetentionSettings == null) {
            return;
        }

        try {
            await settingsService.updateDataRetentionSettings(dataRetentionSettings);
        } catch {
            setHasError(true);
        }
    };

    const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        if (dataRetentionSettings) {
            setDataRetentionSettings({
                ...dataRetentionSettings,
                maxStorageTimeInDays: parseInt(event.target.value, 10),
            });
        }
    };

    useEffect(() => {
        fetchSettings();
    }, []);

    return (
        <LoadingAnimation isLoading={isLoading} hasError={hasError}>
            <Container maxWidth={false}>
                <Grid container spacing={3}>
                    <Grid size={{ xs: 12 }}>
                        <TextField
                            label="Maximum Data Retention Time (Days)"
                            type="number"
                            value={dataRetentionSettings?.maxStorageTimeInDays ?? ""}
                            onChange={handleInputChange}
                            fullWidth
                            slotProps={{
                                htmlInput: {
                                    min: 30
                                }
                            }}
                            helperText="After this many days the archived data for Features is removed."
                        />
                    </Grid>
                    <Grid size={{ xs: 12 }}>
                        <ActionButton
                            buttonVariant="contained"
                            onClickHandler={updateSettings}
                            buttonText="Update Data Retention Settings" />
                    </Grid>
                </Grid>
            </Container>
        </LoadingAnimation>
    );
};

export default DataRetentionSettingsTab;
