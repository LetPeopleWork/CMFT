import { describe, it, expect } from 'vitest';
import { render, screen } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import TeamFeatureList from './TeamFeatureList';
import { Team } from '../../../models/Team';
import { Feature } from '../../../models/Feature';
import { WhenForecast } from '../../../models/Forecasts/WhenForecast';
import { IForecast } from '../../../models/Forecasts/IForecast';

vi.mock('../../../components/Common/Forecasts/ForecastInfoList', () => ({
    default: ({ title, forecasts }: { title: string; forecasts: IForecast[] }) => (
        <div data-testid={`forecast-info-list-${title}`}>
            {forecasts.map((forecast: IForecast, index: number) => (
                <div key={index}>{forecast.probability}%</div>
            ))}
        </div>
    ),
}));

vi.mock('../../../components/Common/LocalDateTimeDisplay/LocalDateTimeDisplay', () => ({
    default: ({ utcDate }: { utcDate: Date }) => <span data-testid="local-date-time-display">{utcDate.toString()}</span>,
}));

describe('FeatureList component', () => {
    const team: Team = new Team(
        "Team A",
        1,
        [],
        [
            new Feature('Feature 1', 1, new Date(), { 0: '' }, { 1: 10 }, {}, [new WhenForecast(80, new Date())]),
            new Feature('Feature 2', 2, new Date(), { 0: '' }, { 1: 5 }, {}, [new WhenForecast(60, new Date())])
        ],
        1
    );

    it('should render all features with correct data', () => {
        render(
            <MemoryRouter>
                <TeamFeatureList team={team} />
            </MemoryRouter>
        );

        team.features.forEach((feature) => {
            const featureNameElement = screen.getByText(feature.name);
            expect(featureNameElement).toBeInTheDocument();

            const remainingWorkElement = screen.getByText(`${feature.getRemainingWorkForTeam(team.id)} / ${feature.getAllRemainingWork()}`);
            expect(remainingWorkElement).toBeInTheDocument();

            const forecastInfoListElements = screen.getAllByTestId((id) => id.startsWith('forecast-info-list-'));
            forecastInfoListElements.map((element) => {
                expect(element).toBeInTheDocument();
            });

            const localDateTimeDisplayElements = screen.getAllByTestId((id) => id.startsWith('local-date-time-display'));
            localDateTimeDisplayElements.map((localDateTimeDisplayElement) => {
                expect(localDateTimeDisplayElement).toBeInTheDocument();
            });
        });
    });

    it('should render the correct number of features', () => {
        render(
            <MemoryRouter>
                <TeamFeatureList team={team} />
            </MemoryRouter>
        );

        const featureRows = screen.getAllByRole('row');
        expect(featureRows).toHaveLength(team.features.length + 1); // Including the header row
    });
});
