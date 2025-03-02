import { GetApiAmmoOptionsRequest } from "@/api/exvs";
import { keepPreviousData, useQuery } from "@tanstack/react-query";
import { ammoApi } from "@/api/api";

export const useApiAmmoOptions = (options: GetApiAmmoOptionsRequest) => {
  return useQuery({
    queryKey: ["getApiAmmoOptions", options],
    queryFn: () => ammoApi.getApiAmmoOptions(options),
    placeholderData: keepPreviousData,
  });
};
