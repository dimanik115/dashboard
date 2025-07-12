import { toValue, type MaybeRefOrGetter } from 'vue';
import type { Statistics, CountryStatistics, SectorStatistics, TypeStatistics } from '../generated';
import { useAxios } from '../utils/useAxios';
import { useQuery } from '@tanstack/vue-query';

const localAxios = useAxios();
localAxios.defaults.baseURL += 'statistics';

async function fetchStatistics(isOnline: boolean): Promise<Statistics[]> {
  return localAxios.get('list', { params: { isOnline: isOnline } }).then((r) => r.data);
}

async function fetchStatisticsByCountry(isOnline: boolean): Promise<CountryStatistics[]> {
  return localAxios.get('byCountry', { params: { isOnline: isOnline } }).then((r) => r.data);
}

async function fetchStatisticsBySector(isOnline: boolean): Promise<SectorStatistics[]> {
  return localAxios.get('bySector', { params: { isOnline: isOnline } }).then((r) => r.data);
}

async function fetchStatisticsByType(isOnline: boolean): Promise<TypeStatistics[]> {
  return localAxios.get('byType', { params: { isOnline: isOnline } }).then((r) => r.data);
}

async function fetchTotalMoney(isOnline: boolean): Promise<number> {
  return localAxios.get('totalMoney', { params: { isOnline: isOnline } }).then((r) => r.data);
}

export function useStatistics(isOnline: MaybeRefOrGetter<boolean>) {
  return useQuery({
    queryKey: [useStatistics.name, isOnline],
    queryFn: () => fetchStatistics(toValue(isOnline)),
  });
}

export function useStatisticsByCountry(isOnline: MaybeRefOrGetter<boolean>) {
  return useQuery({
    queryKey: [useStatisticsByCountry.name, isOnline],
    queryFn: () => fetchStatisticsByCountry(toValue(isOnline)),
  });
}

export function useStatisticsBySector(isOnline: MaybeRefOrGetter<boolean>) {
  return useQuery({
    queryKey: [useStatisticsBySector.name, isOnline],
    queryFn: () => fetchStatisticsBySector(toValue(isOnline)),
  });
}

export function useStatisticsByType(isOnline: MaybeRefOrGetter<boolean>) {
  return useQuery({
    queryKey: [useStatisticsByType.name, isOnline],
    queryFn: () => fetchStatisticsByType(toValue(isOnline)),
  });
}

export function useTotalMoney(isOnline: MaybeRefOrGetter<boolean>) {
  return useQuery({
    queryKey: [useTotalMoney.name, isOnline],
    queryFn: () => fetchTotalMoney(toValue(isOnline)),
  });
}
