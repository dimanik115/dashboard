import type { Statistics } from '../generated';
import { useAxios } from '../utils/useAxios';
import { useQuery } from '@tanstack/vue-query';

const localAxios = useAxios();
localAxios.defaults.baseURL += 'statistics';

async function fetchStatistics(): Promise<Statistics[]> {
  return localAxios.get('list').then((r) => r.data);
}

export function useStatistics() {
  return useQuery({
    queryKey: [useStatistics.name],
    queryFn: () => fetchStatistics(),
  });
}
