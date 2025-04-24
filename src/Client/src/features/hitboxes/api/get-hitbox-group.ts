import { GetApiHitboxGroupsRequest } from "@/api/exvs";
import { queryOptions, useQuery } from "@tanstack/react-query";
import { hitboxesApi } from "@/api/api";
import { QueryConfig } from "@/lib/react-query";

function getHitboxGroups(options: GetApiHitboxGroupsRequest) {
  return hitboxesApi.getApiHitboxGroups(options);
}

export const getHitboxGroupsOptions = (request: GetApiHitboxGroupsRequest) => {
  return queryOptions({
    queryKey: ["getHitboxGroups", request],
    queryFn: () => getHitboxGroups(request),
  });
};

type UseHitboxGroupsOptions = {
  queryConfig?: QueryConfig<typeof getHitboxGroups>;
} & GetApiHitboxGroupsRequest;

export const useHitboxGroups = ({
  queryConfig,
  ...options
}: UseHitboxGroupsOptions = {}) => {
  return useQuery({
    ...getHitboxGroupsOptions(options),
    ...queryConfig,
  });
};
