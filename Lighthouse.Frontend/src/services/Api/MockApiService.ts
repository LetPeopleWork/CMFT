import { Forecast } from '../../models/Forecast';
import { Project } from '../../models/Project';
import { Team } from '../../models/Team';
import { IApiService } from './IApiService';

export class MockApiService implements IApiService {

    getVersion(): Promise<string> {
        return Promise.resolve("v1.33.7");
    }

    getProjectOverviewData(): Promise<Project[]> {
        const binaryBlazer = new Team("Binary Blazers", 1)
        const mavericks = new Team("Mavericks", 2)
        const cyberSultans = new Team("Cyber Sultans", 3)
        const techEagles = new Team("Tech Eagles", 4)

        const lastUpdated = new Date("06/23/2024 12:41");

        const projects: Project[] = [
            new Project("Release 1.33.7", 1, 35, [binaryBlazer], [new Forecast(50, new Date("07/31/2024")), new Forecast(70, new Date("08/05/2024")), new Forecast(85, new Date("08/09/2024")), new Forecast(95, new Date("08/14/2024"))], lastUpdated),
            new Project("Release 42", 2, 28, [mavericks, cyberSultans], [new Forecast(50, new Date("07/09/2024")), new Forecast(70, new Date("07/11/2024")), new Forecast(85, new Date("07/14/2024")), new Forecast(95, new Date("07/17/2024"))], lastUpdated),
            new Project("Release Codename Daniel", 3, 33, [binaryBlazer, techEagles, mavericks, cyberSultans], [new Forecast(50, new Date("07/07/2024")), new Forecast(70, new Date("07/09/2024")), new Forecast(85, new Date("07/12/2024")), new Forecast(95, new Date("07/16/2024"))], lastUpdated)
        ]

        return Promise.resolve(projects);
    }
}