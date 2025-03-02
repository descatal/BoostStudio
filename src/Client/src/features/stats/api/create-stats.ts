import { statsApi } from "@/api/api";
import { CreateStatCommand } from "@/api/exvs";
import { useMutation } from "@tanstack/react-query";

interface useCreateStatsMutationProps {
  onSuccess: () => void;
}

export const useCreateStatsMutation = ({
  onSuccess,
}: useCreateStatsMutationProps) => {
  return useMutation({
    mutationFn: (options: CreateStatCommand) =>
      statsApi.postApiStats({ createStatCommand: options }),
    onSuccess: onSuccess,
  });
};
