import React from "react";
import { useMutation } from "@tanstack/react-query";
import {
  postApiUnitStatsAmmoSlotByIdMutation,
  postApiUnitStatsAmmoSlotMutation,
} from "@/api/exvs/@tanstack/react-query.gen";
import {
  Credenza,
  CredenzaContent,
  CredenzaDescription,
  CredenzaHeader,
  CredenzaTitle,
  CredenzaTrigger,
} from "@/components/credenza";
import { EnhancedButton } from "@/components/ui/enhanced-button";
import { BiPlus, BiSave } from "react-icons/bi";
import { toast } from "sonner";
import { ZodProvider } from "@autoform/zod/v4";
import {
  zCreateUnitAmmoSlotCommand,
  zUpdateUnitAmmoSlotCommand,
} from "@/api/exvs/zod.gen.ts";
import { Label } from "@/components/ui/label.tsx";
import { AutoForm } from "@/components/ui/autoform";
import {
  CreateUnitAmmoSlotCommand,
  UnitAmmoSlotDto,
  UpdateUnitAmmoSlotCommand,
} from "@/api/exvs";
import { Icons } from "@/components/icons.tsx";

interface Props
  extends Omit<React.ComponentPropsWithRef<typeof Credenza>, "children"> {
  index: number;
  unitId?: number | undefined;
  existingData?: UnitAmmoSlotDto | undefined;
  children?: React.ReactNode;
}

const SelectAmmoSlotDialog = ({
  index,
  unitId,
  existingData,
  children,
  ...props
}: Props) => {
  const createMutation = useMutation({
    ...postApiUnitStatsAmmoSlotMutation(),
    onSuccess: (_) => {
      toast("Success", {
        description: `Ammo slot assigned!`,
      });
    },
  });

  const updateMutation = useMutation({
    ...postApiUnitStatsAmmoSlotByIdMutation(),
    onSuccess: (_) => {
      toast("Success", {
        description: `Ammo slot assigned!`,
      });
    },
  });

  const isPending = createMutation.isPending || updateMutation.isPending;

  const handleSubmit = (
    upsertData: CreateUnitAmmoSlotCommand | UpdateUnitAmmoSlotCommand,
  ) => {
    existingData
      ? updateMutation.mutate({
          path: {
            id: (upsertData as UpdateUnitAmmoSlotCommand).id,
          },
          body: upsertData as UpdateUnitAmmoSlotCommand,
        })
      : createMutation.mutate({
          body: upsertData as CreateUnitAmmoSlotCommand,
        });
  };

  const schemaProvider = new ZodProvider(
    existingData ? zUpdateUnitAmmoSlotCommand : zCreateUnitAmmoSlotCommand,
  );

  return (
    <Credenza {...props}>
      <CredenzaTrigger asChild>
        {children ?? (
          <EnhancedButton
            effect={"gooeyRight"}
            icon={BiPlus}
            iconPlacement={"right"}
            variant={"ghost"}
          />
        )}
      </CredenzaTrigger>
      <CredenzaContent>
        <CredenzaHeader>
          <CredenzaTitle>Ammo Slot {index + 1}</CredenzaTitle>
          <CredenzaDescription>
            Select the ammo to be assigned to the ammo slot when spawned.
          </CredenzaDescription>
        </CredenzaHeader>
        <AutoForm
          schema={schemaProvider}
          defaultValues={{
            ...existingData,
            unitId: unitId,
          }}
          onSubmit={(submitData) => {
            handleSubmit({ ...submitData });
          }}
          formProps={{
            className: "px-8 py-4 overflow-auto",
          }}
          uiComponents={{
            FieldWrapper: ({ children, label, error }) => (
              <div className={"py-2"}>
                <Label className={"pb-2"}>{label}</Label>
                {children}
                {error}
              </div>
            ),
            SubmitButton: () => (
              <EnhancedButton
                className={"w-full"}
                effect={"expandIcon"}
                variant={"default"}
                icon={BiSave}
                iconPlacement={"right"}
                disabled={isPending}
              >
                {isPending && (
                  <Icons.spinner
                    className="size-4 mr-2 animate-spin"
                    aria-hidden="true"
                  />
                )}
                Save
              </EnhancedButton>
            ),
          }}
          withSubmit
        />
      </CredenzaContent>
    </Credenza>
  );
};

export default SelectAmmoSlotDialog;
