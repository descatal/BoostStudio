import { tblApi } from "@/api/api";
import { PatchFileVersion } from "@/api/exvs";
import { keepPreviousData, useQuery } from "@tanstack/react-query";

export const useTblById = (id: PatchFileVersion) => {
  return useQuery({
    queryKey: ["getApiTblById", id],
    queryFn: () =>
      tblApi.getApiTblById({
        id: id,
      }),
    placeholderData: keepPreviousData,
  });
};
