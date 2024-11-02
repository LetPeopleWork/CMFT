import React from "react";
import { Link } from "react-router-dom";
import { Tooltip, IconButton } from "@mui/material";
import GppMaybeOutlinedIcon from '@mui/icons-material/GppMaybeOutlined';
import EngineeringIcon from '@mui/icons-material/Engineering';
import { ITeam } from "../../../models/Team/Team";

interface FeatureNameProps {
    name: string;
    url: string;
    isUsingDefaultFeatureSize: boolean;
    teamsWorkIngOnFeature: ITeam[];
}

const FeatureName: React.FC<FeatureNameProps> = ({ name, url, isUsingDefaultFeatureSize, teamsWorkIngOnFeature }) => {
    const teamLinks = teamsWorkIngOnFeature.map((team) => (
        <Link key={team.id} to={`/teams/${team.id}`}>
            {team.name}
        </Link>
    ));

    return (
        <span>
            {url ? (
                <Link to={url} target="_blank" rel="noopener noreferrer">
                    {name}
                </Link>
            ) : (
                name
            )}
            {isUsingDefaultFeatureSize && (
                <Tooltip title="No child items were found for this Feature. The remaining items displayed are based on the default feature size specified in the advanced project settings.">
                    <IconButton size="small" sx={{ ml: 1 }}>
                        <GppMaybeOutlinedIcon sx={{ color: 'warning.main' }} />
                    </IconButton>
                </Tooltip>
            )}
            {teamsWorkIngOnFeature.length > 0 && (
                <Tooltip
                    title={
                        <div>
                            This feature is actively being worked on by: {teamLinks.reduce((acc, curr, index) => (
                                <>
                                    {acc}
                                    {index > 0 && ", "}
                                    {curr}
                                </>
                            ), <></>)} {/* Initial value set to an empty fragment */}
                        </div>
                    }
                >
                    <IconButton size="small" sx={{ ml: 1 }}>
                        <EngineeringIcon />
                    </IconButton>
                </Tooltip>
            )}
        </span>
    );
};

export default FeatureName;