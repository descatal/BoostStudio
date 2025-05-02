import React from "react";
import { ZodProvider } from "@autoform/zod";
import { AutoForm } from "@/components/ui/autoform";
import { AmmoDto, CreateAmmoCommand, UpdateAmmoCommand } from "@/api/exvs";
import { zAmmoDto } from "@/api/exvs/zod.gen";

type AmmoFormProps =
  | {
      data?: undefined;
      children?: React.ReactNode;
      onSubmit: (data: CreateAmmoCommand) => void;
    }
  | {
      data: AmmoDto;
      children?: React.ReactNode;
      onSubmit: (data: UpdateAmmoCommand) => void;
    };

const schemaProvider = new ZodProvider(zAmmoDto);

const AmmoForm = ({ data, onSubmit, children }: AmmoFormProps) => {
  return (
    <AutoForm
      schema={schemaProvider}
      defaultValues={{ ...data, hash: BigInt(data?.hash ?? 0) }}
      onSubmit={(submitData) => {
        data
          ? onSubmit({ ...submitData, hash: Number(submitData.hash!) })
          : onSubmit(submitData);
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

export default AmmoForm;
