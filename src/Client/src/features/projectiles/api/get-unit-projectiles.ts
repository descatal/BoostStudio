import { GetApiUnitProjectilesRequest } from "@/api/exvs";
import { keepPreviousData, useQuery } from "@tanstack/react-query";
import { projectilesApi } from "@/api/api";

export const useApiUnitProjectiles = (
  options: GetApiUnitProjectilesRequest,
) => {
  return useQuery({
    queryKey: ["getApiUnitProjectiles", options],
    queryFn: () => projectilesApi.getApiUnitProjectiles(options),
    placeholderData: keepPreviousData,
  });
};
