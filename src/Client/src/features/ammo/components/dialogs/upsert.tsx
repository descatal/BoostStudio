import React from "react";
import { AmmoDto, CreateAmmoCommand, UpdateAmmoCommand } from "@/api/exvs";
import { useMediaQuery } from "@/hooks/use-media-query";
import { Separator } from "@/components/ui/separator";
import { ChevronDown, PlusIcon } from "lucide-react";
import {
  Drawer,
  DrawerContent,
  DrawerDescription,
  DrawerFooter,
  DrawerHeader,
  DrawerTitle,
  DrawerTrigger,
} from "@/components/ui/drawer";
import { useMutation, useQuery } from "@tanstack/react-query";
import {
  getApiAmmoOptions,
  postApiAmmoByHashMutation,
  postApiAmmoMutation,
} from "@/api/exvs/@tanstack/react-query.gen";
import { EnhancedButton } from "@/components/ui/enhanced-button.tsx";
import {
  Combobox,
  ComboboxAnchor,
  ComboboxContent,
  ComboboxEmpty,
  ComboboxGroup,
  ComboboxGroupLabel,
  ComboboxInput,
  ComboboxItem,
  ComboboxTrigger,
} from "@/components/ui/combobox.tsx";
import { BiSave, BiX } from "react-icons/bi";
import { AutoForm } from "@/components/ui/autoform";
import { Label } from "@/components/ui/label.tsx";
import { Icons } from "@/components/icons.tsx";
import { ZodProvider } from "@autoform/zod/v4";
import { zCreateAmmoCommand, zUpdateAmmoCommand } from "@/api/exvs/zod.gen.ts";
import { UseFormReturn } from "react-hook-form";

interface UpsertAmmoDialogProps
  extends Omit<React.ComponentPropsWithRef<typeof DrawerContent>, "children"> {
  existingData?: AmmoDto | undefined;
  unitId?: number | undefined;
  children?: React.ReactNode;
}

const UpsertAmmoDialog = ({
  existingData,
  children,
  unitId,
  ...props
}: UpsertAmmoDialogProps) => {
  // put this into global state
  const [open, setOpen] = React.useState(false);
  const [data, setData] = React.useState(existingData);
  const formRef = React.useRef<UseFormReturn | null>(null);

  const { data: duplicateOptions } = useQuery({
    ...getApiAmmoOptions({
      query: {
        UnitIds: unitId
          ? [unitId]
          : existingData?.unitId
            ? [existingData.unitId]
            : undefined,
      },
    }),
    select: (data) => {
      return data.items.sort((a, b) => (a.order ?? 0) - (b.order ?? 0)) ?? [];
    },
  });

  const createMutation = useMutation({
    ...postApiAmmoMutation(),
    onSettled: () => () => setOpen(false),
  });

  const updateMutation = useMutation({
    ...postApiAmmoByHashMutation(),
    onSettled: () => () => setOpen(false),
  });

  const isPending = createMutation.isPending || updateMutation.isPending;

  const handleSubmit = (upsertData: CreateAmmoCommand | UpdateAmmoCommand) => {
    existingData
      ? updateMutation.mutate({
          path: {
            hash: (upsertData as UpdateAmmoCommand).hash,
          },
          body: upsertData as UpdateAmmoCommand,
        })
      : createMutation.mutate({ body: upsertData as CreateAmmoCommand });
  };

  const schemaProvider = new ZodProvider(
    existingData ? zUpdateAmmoCommand : zCreateAmmoCommand,
  );
  const isDesktop = useMediaQuery("(min-width: 640px)");

  return (
    <Drawer
      open={open}
      onOpenChange={setOpen}
      direction={isDesktop ? "right" : "bottom"}
    >
      <DrawerTrigger asChild>
        {children ?? (
          <EnhancedButton
            variant="default"
            effect={"ringHover"}
            size={"sm"}
            icon={PlusIcon}
            iconPlacement={"right"}
          >
            Create
          </EnhancedButton>
        )}
      </DrawerTrigger>
      <DrawerContent {...props}>
        <DrawerHeader>
          <div className={"flex flex-row items-center justify-between"}>
            <div>
              <DrawerTitle>{existingData ? "Update" : "Create"}</DrawerTitle>
              <DrawerDescription>
                {existingData ? `Update an existing` : "Create a new"} ammo
                entry <br />
                {existingData && `id: ${existingData.hash}`}
              </DrawerDescription>
            </div>
            <div className={"flex flex-row gap-2"}>
              <Combobox
                onValueChange={(value) => {
                  const matchedData = duplicateOptions?.find(
                    (option) => option.hash === Number(value),
                  );
                  if (matchedData) setData(matchedData);
                }}
              >
                <ComboboxAnchor>
                  <ComboboxInput
                    className={"max-w-30"}
                    placeholder="Duplicate from..."
                  />
                  <ComboboxTrigger>
                    <ChevronDown className="h-4 w-4" />
                  </ComboboxTrigger>
                </ComboboxAnchor>
                <ComboboxContent>
                  <ComboboxEmpty>No other ammo found</ComboboxEmpty>
                  <ComboboxGroup>
                    <ComboboxGroupLabel>Ammo</ComboboxGroupLabel>
                    {duplicateOptions?.map((option) => (
                      <ComboboxItem
                        key={option.hash!}
                        value={option.hash!.toString()}
                      >
                        0x{option.hash!.toString(16)}
                      </ComboboxItem>
                    ))}
                  </ComboboxGroup>
                </ComboboxContent>
              </Combobox>
              <EnhancedButton
                iconPlacement={"right"}
                icon={BiX}
                size={"sm"}
                variant={"outline"}
                onClick={() => {
                  if (formRef.current) {
                    setData({});
                    formRef.current.reset({}, { keepValues: false });
                  }
                }}
              />
            </div>
          </div>
        </DrawerHeader>
        <Separator />
        <AutoForm
          schema={schemaProvider}
          defaultValues={existingData}
          values={data}
          onFormInit={(form) => {
            formRef.current = form;
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
              <DrawerFooter>
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
              </DrawerFooter>
            ),
          }}
          withSubmit
        />
      </DrawerContent>
    </Drawer>
  );
};

export default UpsertAmmoDialog;
