import React from "react";
import { CreateStatCommand, StatDto, UpdateStatCommand } from "@/api/exvs/zod";
import { ZodProvider } from "@autoform/zod";
import { AutoForm } from "@/components/ui/autoform";

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

const schemaProvider = new ZodProvider(StatDto);

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
