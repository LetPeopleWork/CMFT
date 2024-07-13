import { WhenForecast } from '../../models/Forecasts/WhenForecast';
import { Project } from '../../models/Project';
import { Team } from '../../models/Team';
import { IApiService } from './IApiService';
import { Feature } from '../../models/Feature';
import { Throughput } from '../../models/Forecasts/Throughput';
import { ManualForecast } from '../../models/Forecasts/ManualForecast';
import { HowManyForecast } from '../../models/Forecasts/HowManyForecast';
import dayjs from 'dayjs';
import { Milestone } from '../../models/Milestone';

export class MockApiService implements IApiService {
    private useDelay: boolean;
    private throwError: boolean;

    private lastUpdated = new Date("06/23/2024 12:41");

    private feature1 = new Feature('Feature 1', 1, new Date(), { 1: 10 }, [new WhenForecast(50, new Date("07/31/2024")), new WhenForecast(70, new Date("08/05/2024")), new WhenForecast(85, new Date("08/09/2024")), new WhenForecast(95, new Date("08/14/2024"))]);
    private feature2 = new Feature('Feature 2', 2, new Date(), { 2: 5 }, [new WhenForecast(50, new Date("07/09/2024")), new WhenForecast(70, new Date("07/11/2024")), new WhenForecast(85, new Date("07/14/2024")), new WhenForecast(95, new Date("07/17/2024"))]);
    private feature3 = new Feature('Feature 3', 3, new Date(), { 3: 7, 2: 15 }, [new WhenForecast(50, new Date("07/07/2024")), new WhenForecast(70, new Date("07/09/2024")), new WhenForecast(85, new Date("07/12/2024")), new WhenForecast(95, new Date("07/16/2024"))]);
    private feature4 = new Feature('Feature 4', 4, new Date(), { 1: 3, 4: 9 }, [new WhenForecast(50, new Date("07/31/2024")), new WhenForecast(70, new Date("08/05/2024")), new WhenForecast(85, new Date("08/09/2024")), new WhenForecast(95, new Date("08/14/2024"))]);

    private binaryBlazers = new Team("Binary Blazers", 1, [], [this.feature1, this.feature4], 1);
    private mavericks = new Team("Mavericks", 2, [], [this.feature2, this.feature3], 2);
    private cyberSultans = new Team("Cyber Sultans", 3, [], [this.feature3], 1);
    private techEagles = new Team("Tech Eagles", 4, [], [this.feature4], 2);

    private dayMultiplier : number = 24 * 60 * 60 * 1000;

    private milestone1 = new Milestone("Milestone 1", new Date(Date.now() + 14 * this.dayMultiplier));
    private milestone2 = new Milestone("Milestone 1", new Date(Date.now() + 28 * this.dayMultiplier));
    private milestone3 = new Milestone("Milestone 1", new Date(Date.now() + 90 * this.dayMultiplier));

    private release_1337 = new Project("Release 1.33.7", 1, [this.binaryBlazers], [this.feature1], [], this.lastUpdated);
    private release_42 = new Project("Release 42", 2, [this.mavericks], [this.feature2], [this.milestone1], this.lastUpdated);
    private release_codename_daniel = new Project("Release Codename Daniel", 3, [this.binaryBlazers, this.techEagles, this.mavericks, this.cyberSultans], [this.feature3, this.feature4], [this.milestone2, this.milestone3], this.lastUpdated);


    constructor(useDelay: boolean, throwError: boolean = false) {
        this.useDelay = useDelay;
        this.throwError = throwError;
    }

    async updateThroughput(teamId: number): Promise<void> {
        console.log(`Updating Throughput for Team ${teamId}`);

        await this.delay();
    }

    async getThroughput(teamId: number): Promise<Throughput> {
        console.log(`Getting Throughput for Team ${teamId}`);

        await this.delay();

        const randomThroughput = this.generateThroughput();
        return new Throughput(randomThroughput);
    }

    async updateForecast(teamId: number): Promise<void> {
        console.log(`Updating Forecast for Team ${teamId}`);

        await this.delay();
    }

    async runManualForecast(teamId: number, remainingItems: number, targetDate: Date): Promise<ManualForecast> {
        console.log(`Updating Forecast for Team ${teamId}: How Many: ${remainingItems} - When: ${targetDate}`);
        await this.delay();

        const howManyForecasts = [
            new HowManyForecast(50, 42), new HowManyForecast(70, 31), new HowManyForecast(85, 12), new HowManyForecast(95, 7)
        ]

        const whenForecasts = [
            new WhenForecast(50, dayjs().add(2, 'days').toDate()), new WhenForecast(70, dayjs().add(5, 'days').toDate()), new WhenForecast(85, dayjs().add(9, 'days').toDate()), new WhenForecast(95, dayjs().add(12, 'days').toDate())
        ]

        const likelihood = Math.round(Math.random() * 10000) / 100;

        return new ManualForecast(remainingItems, targetDate, whenForecasts, howManyForecasts, likelihood);
    }

    async getTeams(): Promise<Team[]> {
        await this.delay();

        return [
            this.binaryBlazers,
            this.mavericks,
            this.cyberSultans,
            this.techEagles,
        ];
    }

    async getTeam(id: number): Promise<Team | null> {
        console.log(`Getting Team with id ${id}`)
        const teams = await this.getTeams();
        const team = teams.find(team => team.id === id);
        return team || null;
    }

    async deleteTeam(id: number): Promise<void> {
        console.log(`'Deleting' Team with id ${id}`)
        await this.delay();
    }
    
    async getProject(id: number): Promise<Project | null> {
        console.log(`Getting Project with id ${id}`)
        const projects = await this.getProjects();
        const project = projects.find(project => project.id === id);
        return project || null;
    }

    async deleteProject(id: number): Promise<void> {
        console.log(`'Deleting' Project with id ${id}`)
        await this.delay();

    }

    async getVersion(): Promise<string> {
        await this.delay()
        return "v1.33.7";
    }

    async getProjects(): Promise<Project[]> {
        await this.delay();

        return [this.release_1337, this.release_42, this.release_codename_daniel];
    }

    delay() {
        if (this.throwError) {
            throw new Error('Simulated Error');
        }

        if (this.useDelay) {
            const randomDelay: number = Math.random() * 1000;
            return new Promise(resolve => setTimeout(resolve, randomDelay));
        }

        return Promise.resolve();
    }

    generateThroughput(): number[] {
        const length = Math.floor(Math.random() * (90 - 15 + 1)) + 15;
        const randomArray: number[] = [];

        for (let i = 0; i < length; i++) {
            const randomNumber = Math.floor(Math.random() * 5);
            randomArray.push(randomNumber);
        }

        return randomArray;
    }
}