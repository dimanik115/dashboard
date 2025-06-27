import DefaultConfig from '../../../public/config.json';

type Configuration = typeof DefaultConfig;

const defaultConfig = DefaultConfig;

let fetchedConfig = {};

export const setConfiguration = (configuration: Configuration) => (fetchedConfig = configuration);
export const useConfiguration = (): Configuration => ({ ...defaultConfig, ...fetchedConfig });
