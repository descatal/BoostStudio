import { GetApiHitboxesRequest, GetApiHitboxGroupsRequest } from "@/api/exvs";
import { keepPreviousData, useQuery } from "@tanstack/react-query";
import { hitboxesApi } from "@/api/api";

export const useApiHitboxGroups = (options: GetApiHitboxGroupsRequest) => {
  return useQuery({
    queryKey: ["getApiHitboxGroups", options],
    queryFn: () => hitboxesApi.getApiHitboxGroups(options),
    placeholderData: keepPreviousData,
  });
};
