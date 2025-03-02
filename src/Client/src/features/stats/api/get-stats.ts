import { GetApiStatsRequest } from "@/api/exvs";
import { keepPreviousData, useQuery } from "@tanstack/react-query";
import { statsApi } from "@/api/api";

export const useApiUnitStatsGroup = (options: GetApiStatsRequest) => {
  return useQuery({
    queryKey: ["getApiStats", options],
    queryFn: () => statsApi.getApiStats(options),
    placeholderData: keepPreviousData,
  });
};
