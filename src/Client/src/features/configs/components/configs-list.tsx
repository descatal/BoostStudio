import { useMutation, useQueries } from "@tanstack/react-query";
import {
  getApiConfigsByKeyOptions,
  postApiConfigsMutation,
} from "@/api/exvs/@tanstack/react-query.gen.ts";
import React from "react";
import {
  Editable,
  EditableArea,
  EditableInput,
  EditablePreview,
  EditableTrigger,
} from "@/components/ui/editable.tsx";
import { EnhancedButton } from "@/components/ui/enhanced-button.tsx";
import { Edit, Trash2 } from "lucide-react";
import { cn } from "@/lib/utils";
import { Label } from "@/components/ui/label.tsx";
import { toast } from "sonner";

interface Props extends React.HTMLProps<HTMLDivElement> {
  configOptions: {
    label: string;
    key: string;
    description: string;
    defaultValue: string;
  }[];
}

const ConfigsList = ({ configOptions, ...props }: Props) => {
  const queries = useQueries({
    queries: configOptions.map((opt) => ({
      ...getApiConfigsByKeyOptions({
        path: {
          key: opt.key,
        },
      }),
    })),
    combine: (results) => {
      // returned results will be in the same order, so we can map them into the result we want
      return {
        data: results.map((result, index) => ({
          ...configOptions[index],
          data: result.data,
        })),
        pending: results.some((result) => result.isPending),
      };
    },
  });

  const data = queries.data;

  const mutation = useMutation({
    ...postApiConfigsMutation(),
    onSuccess: () => {
      toast.success("Configuration saved successfully!");
    },
  });

  return (
    <div {...props}>
      {!queries.pending &&
        data.map((opt) => (
          <div className={"flex flex-col gap-2"}>
            <div className={"flex flex-col gap-2"}>
              <Label className={"leading-none font-semibold"} htmlFor={opt.key}>
                {opt.label}
              </Label>
              <Label
                className={"text-muted-foreground text-sm"}
                htmlFor={opt.key}
              >
                {opt.description}
              </Label>
            </div>
            <div
              id={opt.key}
              key={opt.key}
              className="flex items-center gap-2 rounded-lg border bg-card px-2 py-2"
            >
              <Editable
                key={opt.key}
                defaultValue={opt.data ?? opt.defaultValue}
                onSubmit={(value) => {
                  mutation.mutate({
                    body: {
                      key: opt.key,
                      value: value,
                    },
                  });
                }}
                className="flex flex-1 flex-row items-center gap-1.5"
              >
                <EditableArea className="flex-1">
                  <EditablePreview
                    className={cn("w-full rounded-md px-1.5 py-1")}
                  />
                  <EditableInput
                    placeholder={opt.defaultValue}
                    className="px-1.5 py-1"
                  />
                </EditableArea>
                <EditableTrigger asChild>
                  <EnhancedButton
                    variant="ghost"
                    size="icon"
                    className="size-7"
                  >
                    <Edit />
                  </EnhancedButton>
                </EditableTrigger>
              </Editable>
              <EnhancedButton
                variant="ghost"
                size="icon"
                className="size-7 text-destructive"
                onClick={() => {}}
              >
                <Trash2 />
              </EnhancedButton>
            </div>
          </div>
        ))}
    </div>
  );
};

export default ConfigsList;
