import React from "react";
import { ZodProvider } from "@autoform/zod";
import { AutoForm } from "@/components/ui/autoform";
import { CreateStatCommand, StatDto, UpdateStatCommand } from "@/api/exvs";
import { zStatDto } from "@/api/exvs/zod.gen";

type StatsGroupFormProps =
  | {
      data?: undefined;
      children?: React.ReactNode;
      onSubmit: (data: CreateStatCommand) => void;
    }
  | {
      data: StatDto;
      children?: React.ReactNode;
      onSubmit: (data: UpdateStatCommand) => void;
    };

const schemaProvider = new ZodProvider(zStatDto);

const StatsGroupForm = ({ data, children, onSubmit }: StatsGroupFormProps) => {
  return (
    <AutoForm
      schema={schemaProvider}
      defaultValues={data}
      onSubmit={(submitData) => {
        onSubmit({ ...submitData, id: submitData.id! });
      }}
      formProps={{
        className: "px-2 overflow-auto",
      }}
      withSubmit={!children}
    >
      {children}
    </AutoForm>
  );
};

export default StatsGroupForm;
