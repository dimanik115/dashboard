import type { Bookmark } from '../generated';
import { useAxios } from '../utils/useAxios';
import { useQuery } from '@tanstack/vue-query';

const localAxios = useAxios();
localAxios.defaults.baseURL += 'bookmarks';

async function fetchBookmarks(): Promise<Bookmark[]> {
  return localAxios.get('list').then((r) => r.data);
}

export function useBookmarks() {
  return useQuery({
    queryKey: [useBookmarks.name],
    queryFn: () => fetchBookmarks(),
  });
}
