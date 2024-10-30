import React from 'react';
import { AppBar, Toolbar, Box } from '@mui/material';
import LighthouseLogo from '../LetPeopleWork/LighthouseLogo';
import NavigationItem from './NavigationItem';
import ExternalLinkButton from './ExternalLinkButton';
import { BugReport } from '@mui/icons-material';
import GitHubIcon from '@mui/icons-material/GitHub';
import YouTubeIcon from '@mui/icons-material/YouTube';
import BlogIcon from '@mui/icons-material/RssFeed';
import HandshakeIcon from '@mui/icons-material/Handshake';

const Header: React.FC = () => {
  return (
    <AppBar position="static" className="header" style={{ backgroundColor: 'white' }}>
      <Toolbar className="toolbar">
        <Box className="logo">
          <LighthouseLogo />
        </Box>
        <Box className="nav-links">
          <NavigationItem path='/' text='Overview' />
          <NavigationItem path='/teams' text='Teams' />
          <NavigationItem path='/projects' text='Projects' />
          <NavigationItem path='/settings' text='Settings' />
        </Box>
        <Box sx={{ display: { xs: 'none', md: 'flex' } }}>
          <ExternalLinkButton
            link="https://github.com/LetPeopleWork/Lighthouse/blob/main/CONTRIBUTORS.md"
            icon={HandshakeIcon}
            tooltip='Contributors'
          />
          <ExternalLinkButton
            link="https://github.com/LetPeopleWork/Lighthouse/issues"
            icon={BugReport}
            tooltip='Report an Issue'
          />
          <ExternalLinkButton
            link="https://www.youtube.com/channel/UCipDDn2dpVE3rpoKNW2asZQ"
            icon={YouTubeIcon}
            tooltip='Check out our YouTube Channel'
          />
          <ExternalLinkButton
            link="https://www.letpeople.work/blog/"
            icon={BlogIcon}
            tooltip='Read our Blog'
          />
          <ExternalLinkButton
            link="https://github.com/LetPeopleWork/"
            icon={GitHubIcon}
            tooltip='View our OpenSource Projects on GitHub'
          />
        </Box>
      </Toolbar>
    </AppBar>
  );
};

export default Header;
