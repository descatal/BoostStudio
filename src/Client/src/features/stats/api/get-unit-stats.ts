import { statsApi } from "@/api/api";
import { GetApiUnitStatsRequest } from "@/api/exvs";
import { keepPreviousData, useQuery } from "@tanstack/react-query";

export const useApiUnitStats = (options: GetApiUnitStatsRequest) => {
  return useQuery({
    queryKey: ["getApiUnitStats", options],
    queryFn: () => statsApi.getApiUnitStats(options),
    placeholderData: keepPreviousData,
  });
};
