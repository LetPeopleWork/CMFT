import React from 'react';
import { NavLink } from 'react-router-dom';
import { SvgIconComponent } from "@mui/icons-material";

interface StyledNavLinkProps {
    link: string;
    text: string;
    icon: SvgIconComponent;
    isTitle?: boolean;
}

const StyleCardNavLink: React.FC<StyledNavLinkProps> = ({ link, text, icon: Icon, isTitle = false }) => {
    return (
        <NavLink to={link} style={{
            display: 'flex',
            alignItems: 'center',
            textDecoration: 'none',
            color: 'inherit',
            fontSize: isTitle ? '1.5rem' : 'inherit',
            fontWeight: isTitle ? 'bold' : 'normal',
        }}>
            <Icon style={{ color: 'rgba(48, 87, 78, 1)', marginRight: 8 }} data-testid="styled-card-icon" />
            {text}
        </NavLink>
    );
}

export default StyleCardNavLink;
