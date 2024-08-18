import axios from 'axios';
import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest';
import { LogService } from './LogService';

vi.mock('axios');
const mockedAxios = vi.mocked(axios, true);

describe('LogService', () => {
    let logService: LogService;    

    beforeEach(() => {
        mockedAxios.create.mockReturnThis();
        logService = new LogService();
    });

    afterEach(() => {
        vi.resetAllMocks();
    });

    it('should get supported log levels', async () => {
        const mockResponse = ['info', 'warn', 'error'];
        mockedAxios.get.mockResolvedValueOnce({ data: mockResponse });

        const logLevels = await logService.getSupportedLogLevels();

        expect(logLevels).toEqual(mockResponse);
        expect(mockedAxios.get).toHaveBeenCalledWith('/logs/level/supported');
    });

    it('should get log level', async () => {
        const mockResponse = 'info';
        mockedAxios.get.mockResolvedValueOnce({ data: mockResponse });

        const logLevel = await logService.getLogLevel();

        expect(logLevel).toEqual(mockResponse);
        expect(mockedAxios.get).toHaveBeenCalledWith('/logs/level');
    });

    it('should set log level', async () => {
        mockedAxios.post.mockResolvedValueOnce({});

        await logService.setLogLevel('error');

        expect(mockedAxios.post).toHaveBeenCalledWith('/logs/level', { level: 'error' });
    });

    it('should get logs', async () => {
        const mockResponse = 'Log data';
        mockedAxios.get.mockResolvedValueOnce({ data: mockResponse });

        const logs = await logService.getLogs();

        expect(logs).toEqual(mockResponse);
        expect(mockedAxios.get).toHaveBeenCalledWith('/logs');
    });
});