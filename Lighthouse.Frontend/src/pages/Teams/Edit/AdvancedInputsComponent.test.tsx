import { render, screen, fireEvent } from '@testing-library/react';
import { describe, it, expect, vi } from 'vitest';
import { ITeamSettings, TeamSettings } from '../../../models/Team/TeamSettings';
import AdvancedInputsComponent from './AdvancedInputs';

describe('AdvancedInputsComponent', () => {
    it('renders correctly with provided teamSettings', () => {
        const teamSettings: ITeamSettings = new TeamSettings(0, "setting", 2, 20, "", [], 12, "Test Field");

        render(
            <AdvancedInputsComponent
                teamSettings={teamSettings}
                onTeamSettingsChange={vi.fn()}
            />
        );

        // Check if the TextField displays the correct value
        expect(screen.getByLabelText('Relation Custom Field')).toHaveValue('Test Field');        
        expect(screen.getByLabelText('Feature WIP')).toHaveValue(20);
    });

    it('calls onTeamSettingsChange with the correct parameters when the TextField value changes', () => {
        const onTeamSettingsChange = vi.fn();
        const teamSettings: ITeamSettings = new TeamSettings(0, "setting", 2, 2, "", [], 12, "");

        render(
            <AdvancedInputsComponent
                teamSettings={teamSettings}
                onTeamSettingsChange={onTeamSettingsChange}
            />
        );

        // Simulate typing in the TextField
        fireEvent.change(screen.getByLabelText('Relation Custom Field'), {
            target: { value: 'New Value' }
        });

        // Check if the callback was called with the correct parameters
        expect(onTeamSettingsChange).toHaveBeenCalledWith('relationCustomField', 'New Value');
    });

    it('calls onTeamSettingsChange with correct parameters when Feature WIP TextField value changes', () => {
        const onTeamSettingsChange = vi.fn();
        const teamSettings: ITeamSettings = new TeamSettings(0, "setting", 2, 2, "", [], 12, "");

        render(
            <AdvancedInputsComponent
                teamSettings={teamSettings}
                onTeamSettingsChange={onTeamSettingsChange}
            />
        );

        fireEvent.change(screen.getByLabelText('Feature WIP'), {
            target: { value: '25' }
        });

        expect(onTeamSettingsChange).toHaveBeenCalledWith('featureWIP', 25);
    });
});
