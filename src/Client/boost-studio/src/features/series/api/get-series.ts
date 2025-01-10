import {queryOptions} from "@tanstack/react-query";



// export const getSeriesQueryOptions = (
//   {
//     page,
//   }: { page?: number } = {}) => {
//   return queryOptions({
//     queryKey: page ? ['discussions', {page}] : ['discussions'],
//     queryFn: () => getDiscussions(page),
//   });
// };