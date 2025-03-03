import { GetApiProjectilesRequest } from "@/api/exvs";
import { keepPreviousData, useQuery } from "@tanstack/react-query";
import { projectilesApi } from "@/api/api";

export const useApiProjectiles = (options: GetApiProjectilesRequest) => {
  return useQuery({
    queryKey: ["getApiProjectiles", options],
    queryFn: () => projectilesApi.getApiProjectiles(options),
    placeholderData: keepPreviousData,
  });
};
