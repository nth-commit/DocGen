export interface AppSettings {
    Urls: {
        Api: string;
    };
}

export const getAppSettings = (): AppSettings => window['DOCGEN_ENVIRONMENT_SETTINGS'];
