import { GetApiHitboxesRequest } from "@/api/exvs";
import { keepPreviousData, useQuery } from "@tanstack/react-query";
import { hitboxesApi } from "@/api/api";

export const useApiHitboxes = (options: GetApiHitboxesRequest) => {
  return useQuery({
    queryKey: ["getApiHitboxes", options],
    queryFn: () => hitboxesApi.getApiHitboxes(options),
    placeholderData: keepPreviousData,
  });
};
