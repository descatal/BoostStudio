import { GetApiUnitStatsByUnitIdRequest } from "@/api/exvs";
import { keepPreviousData, useQuery } from "@tanstack/react-query";
import { statsApi } from "@/api/api";

export const useApiUnitStatsByUnitId = (
  options: GetApiUnitStatsByUnitIdRequest,
) => {
  return useQuery({
    queryKey: ["getApiUnitStatsByUnitId", options],
    queryFn: () => statsApi.getApiUnitStatsByUnitId(options),
    placeholderData: keepPreviousData,
  });
};
