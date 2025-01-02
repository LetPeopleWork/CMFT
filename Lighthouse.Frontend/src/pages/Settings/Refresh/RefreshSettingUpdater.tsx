import { Container, TextField } from "@mui/material";
import Grid from "@mui/material/Grid2";
import type React from "react";
import { useContext, useEffect, useState } from "react";
import ActionButton from "../../../components/Common/ActionButton/ActionButton";
import LoadingAnimation from "../../../components/Common/LoadingAnimation/LoadingAnimation";
import type { IRefreshSettings } from "../../../models/AppSettings/RefreshSettings";
import { ApiServiceContext } from "../../../services/Api/ApiServiceContext";

interface RefreshSettingUpdaterProps {
	settingName: string;
}

const RefreshSettingUpdater: React.FC<RefreshSettingUpdaterProps> = ({
	settingName,
}) => {
	const [refreshSettings, setRefreshSettings] =
		useState<IRefreshSettings | null>(null);
	const [isLoading, setIsLoading] = useState<boolean>(false);
	const [hasError, setHasError] = useState<boolean>(false);

	const { settingsService } = useContext(ApiServiceContext);

	const updateSettings = async () => {
		if (refreshSettings == null) {
			return;
		}

		await settingsService.updateRefreshSettings(settingName, refreshSettings);
	};

	useEffect(() => {
		const fetchData = async () => {
			setIsLoading(true);
			setHasError(false);

			try {
				const loadedSettings =
					await settingsService.getRefreshSettings(settingName);
				setRefreshSettings(loadedSettings);
			} catch {
				setHasError(true);
			} finally {
				setIsLoading(false);
			}
		};

		fetchData();
	}, [settingsService, settingName]);

	const handleInputChange =
		(field: keyof IRefreshSettings) =>
		(event: React.ChangeEvent<HTMLInputElement>) => {
			if (refreshSettings) {
				setRefreshSettings({
					...refreshSettings,
					[field]: Number.parseInt(event.target.value, 10),
				});
			}
		};

	return (
		<LoadingAnimation isLoading={isLoading} hasError={hasError}>
			<Container maxWidth={false}>
				<Grid container spacing={3}>
					<Grid size={{ xs: 12 }}>
						<TextField
							label="Interval (Minutes)"
							type="number"
							value={refreshSettings?.interval ?? ""}
							onChange={handleInputChange("interval")}
							fullWidth
							slotProps={{
								htmlInput: {
									min: 1,
								},
							}}
						/>
					</Grid>
					<Grid size={{ xs: 12 }}>
						<TextField
							label="Refresh After (Minutes)"
							type="number"
							value={refreshSettings?.refreshAfter ?? ""}
							onChange={handleInputChange("refreshAfter")}
							fullWidth
							slotProps={{
								htmlInput: {
									min: 1,
								},
							}}
						/>
					</Grid>
					<Grid size={{ xs: 12 }}>
						<TextField
							label="Start Delay (Minutes)"
							type="number"
							value={refreshSettings?.startDelay ?? ""}
							onChange={handleInputChange("startDelay")}
							fullWidth
							slotProps={{
								htmlInput: {
									min: 1,
								},
							}}
						/>
					</Grid>
					<Grid size={{ xs: 12 }}>
						<ActionButton
							buttonVariant="contained"
							onClickHandler={updateSettings}
							buttonText={`Update ${settingName} Settings`}
						/>
					</Grid>
				</Grid>
			</Container>
		</LoadingAnimation>
	);
};

export default RefreshSettingUpdater;
