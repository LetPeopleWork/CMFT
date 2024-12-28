import { APIRequestContext, test as base } from '@playwright/test';
import { LighthousePage } from '../models/app/LighthousePage';
import { OverviewPage } from '../models/overview/OverviewPage';
import { createAzureDevOpsConnection, createJiraConnection, deleteWorkTrackingSystemConnection } from '../helpers/api/workTrackingSystemConnections';
import { createTeam, deleteTeam, updateTeam } from '../helpers/api/teams';
import { createProject, deleteProject } from '../helpers/api/projects';
import { generateRandomName } from '../helpers/names';

type LighthouseFixtures = {
    overviewPage: OverviewPage;
};

type LighthouseWithDataFixtures = {
    testData: TestData;
}

type TestData = { projects: ModelIdentifier[], teams: ModelIdentifier[], connections: ModelIdentifier[] };

type ModelIdentifier = {
    id: number,
    name: string
}

async function deleteTestData(request: APIRequestContext, testData: TestData): Promise<void> {
    for (const project of testData.projects) {
        await deleteProject(request, project.id);
    }

    for (const team of testData.teams) {
        await deleteTeam(request, team.id);
    }

    for (const connection of testData.connections) {
        await deleteWorkTrackingSystemConnection(request, connection.id);
    }
}

async function generateTestData(request: APIRequestContext, updateTeams: boolean): Promise<TestData> {
    const adoConnection = await createAzureDevOpsConnection(request, generateRandomName());
    const jiraConnection = await createJiraConnection(request, generateRandomName());

    const adoStates = { toDo: ['New'], doing: ['Active', 'Resolved'], done: ['Closed'] };
    const jiraStates = { toDo: ['To Do'], doing: ['In Progress'], done: ['Done'] };

    const team1 = await createTeam(request, generateRandomName(), adoConnection.id, '[System.TeamProject] = "Lighthouse Demo" AND [System.AreaPath] = "Lighthouse Demo\\Binary Blazers"', ['User Story', 'Bug'], adoStates);
    const team2 = await createTeam(request, generateRandomName(), adoConnection.id, '[System.TeamProject] = "Lighthouse Demo" AND [System.AreaPath] = "Lighthouse Demo\\Cyber Sultans"', ['User Story', 'Bug'], adoStates);
    const team3 = await createTeam(request, generateRandomName(), jiraConnection.id, 'project = "LGHTHSDMO" AND labels = "Lagunitas"', ['Story', 'Bug'], jiraStates);

    if (updateTeams) {
        await updateTeam(request, team1.id);
        await updateTeam(request, team2.id);
        await updateTeam(request, team3.id);
    }

    const project1 = await createProject(request, generateRandomName(), [team1], adoConnection.id, '[System.TeamProject] = "Lighthouse Demo" AND [System.Tags] CONTAINS "Release 1.33.7"', ["Epic"], adoStates);
    const project2 = await createProject(request, generateRandomName(), [team1, team2], adoConnection.id, '[System.TeamProject] = "Lighthouse Demo" AND [System.Tags] CONTAINS "Release Codename Daniel"', ["Epic"], adoStates);
    const project3 = await createProject(request, generateRandomName(), [team3], jiraConnection.id, 'project = "LGHTHSDMO" AND fixVersion = "Oberon Initiative"', ["Epic"], jiraStates);

    return {
        projects: [project1, project2, project3],
        teams: [team1, team2, team3],
        connections: [adoConnection, jiraConnection]
    }
}

export const test = base.extend<LighthouseFixtures>({
    overviewPage: async ({ page }, use) => {
        const lighthousePage = new LighthousePage(page);
        const overviewPage = await lighthousePage.open();

        await use(overviewPage);
    },
});

export const testWithUpdatedTeams = test.extend<LighthouseWithDataFixtures>({
    testData: async ({ request }, use) => {
        const data = await generateTestData(request, true);

        await use(data);

        await deleteTestData(request, data);
    },
});

export const testWithData = test.extend<LighthouseWithDataFixtures>({
    testData: async ({ request }, use) => {
        const data = await generateTestData(request, false);

        await use(data);

        await deleteTestData(request, data);
    },
});

export { expect } from '@playwright/test';