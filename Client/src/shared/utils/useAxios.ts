import { useConfiguration } from '@/shared/utils/useConfig.ts';
import axios from 'axios';

/**
 * возвращает сконфигурированный инстанс аксиоса
 */
export const useAxios = (path: string = 'api/v1/') => {
  const baseURL = useConfiguration().urls.api;
  const instance = axios.create({
    baseURL: baseURL + path,
    headers: { 'X-Custom-Header': 'foobar' },
  });
  axios.interceptors.request.use((config) => {
    config.headers.Authorization = 'Bearer {token}';
    return config;
  });
  return instance;
};

export function useTinkAxios() {
  const baseURL = 'https://sandbox-invest-public-api.tinkoff.ru:443';
  const instance = axios.create({
    baseURL: baseURL,
    headers: { Authorization: 'Bearer ' + import.meta.env.VITE_TOKEN },
  });
  return instance;
}
