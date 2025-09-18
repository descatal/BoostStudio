import React from "react";
import { AmmoDto, CreateAmmoCommand, UpdateAmmoCommand } from "@/api/exvs";
import { useMediaQuery } from "@/hooks/use-media-query";
import { Separator } from "@/components/ui/separator";
import { ChevronDown, PlusIcon } from "lucide-react";
import {
  Drawer,
  DrawerClose,
  DrawerContent,
  DrawerDescription,
  DrawerFooter,
  DrawerHeader,
  DrawerTitle,
  DrawerTrigger,
} from "@/components/ui/drawer";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import {
  getApiAmmoOptions,
  getApiAmmoQueryKey,
  postApiAmmoByHashMutation,
  postApiAmmoMutation,
} from "@/api/exvs/@tanstack/react-query.gen";
import { EnhancedButton } from "@/components/ui/enhanced-button.tsx";
import {
  Combobox,
  ComboboxAnchor,
  ComboboxContent,
  ComboboxEmpty,
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
import { toast } from "sonner";

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
  const [duplicateSelection, setDuplicateSelection] = React.useState("");
  const [duplicateInput, setDuplicateInput] = React.useState("");

  const queryClient = useQueryClient();
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
    onSuccess: onSuccess,
  });

  const updateMutation = useMutation({
    ...postApiAmmoByHashMutation(),
    onSuccess: onSuccess,
  });

  async function onSuccess() {
    toast("Success", {
      description: `${existingData ? "Update" : "Create"} success!`,
    });

    setOpen(false);

    await queryClient.invalidateQueries({
      predicate: (query) =>
        // @ts-ignore
        query.queryKey[0]._id === getApiAmmoQueryKey()[0]._id,
    });
  }

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

  React.useEffect(() => {
    const matchedData = duplicateOptions?.find(
      (option) =>
        option.hash!.toString(16).toUpperCase() === duplicateSelection,
    );
    if (matchedData) setData(matchedData);
  }, [duplicateSelection]);

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
        <DrawerHeader className="px-6 pt-6 pb-4 border-b">
          <div className={"flex flex-row items-center justify-between"}>
            <div className="space-y-4">
              <div className="flex items-center justify-between">
                <div className="space-y-1">
                  <DrawerTitle>
                    {existingData ? "Update" : "Create"} Ammo
                  </DrawerTitle>
                  <DrawerDescription>
                    {existingData
                      ? `Update the ammo information below.`
                      : "Fill out the ammo information below."}
                  </DrawerDescription>
                </div>
              </div>
            </div>
          </div>
          {existingData && (
            <div className="flex items-center gap-2 px-3 py-2 bg-muted rounded-md">
              <span className="text-sm font-medium text-muted-foreground">
                ID:
              </span>
              <code className="text-sm font-mono bg-background px-2 py-1 rounded border">
                {existingData.hash}
              </code>
            </div>
          )}
          <div className="flex items-center gap-2 w-full pt-2">
            <Combobox
              className={"w-full"}
              inputValue={duplicateInput}
              onInputValueChange={setDuplicateInput}
              value={duplicateSelection}
              onValueChange={setDuplicateSelection}
            >
              <ComboboxAnchor>
                <ComboboxInput placeholder="Duplicate from..." />
                <ComboboxTrigger>
                  <ChevronDown className="h-4 w-4" />
                </ComboboxTrigger>
              </ComboboxAnchor>
              <ComboboxContent>
                <ComboboxEmpty>No other ammo found</ComboboxEmpty>
                {duplicateOptions?.map((option) => (
                  <ComboboxItem
                    key={option.hash!}
                    value={option.hash!.toString(16).toUpperCase()}
                  >
                    {option.hash!.toString(16).toUpperCase()}
                  </ComboboxItem>
                ))}
              </ComboboxContent>
            </Combobox>
            <EnhancedButton
              className={"w-fit"}
              variant="outline"
              size="sm"
              type={"reset"}
              icon={BiX}
              effect={"expandIcon"}
              iconPlacement={"right"}
              form={"form"}
              onClick={() => {
                setDuplicateSelection("");
                setDuplicateInput("");
                setData(undefined);
              }}
            >
              Clear
            </EnhancedButton>
          </div>
        </DrawerHeader>
        <Separator />
        <AutoForm
          schema={schemaProvider}
          defaultValues={existingData}
          values={data}
          onSubmit={(submitData) => {
            handleSubmit({ ...submitData });
          }}
          formProps={{
            id: "form",
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
          }}
        />
        <DrawerFooter>
          <EnhancedButton
            effect={"expandIcon"}
            variant={"default"}
            icon={BiSave}
            iconPlacement={"right"}
            disabled={isPending}
            form={"form"}
            type={"submit"}
          >
            {isPending && (
              <Icons.spinner
                className="size-4 mr-2 animate-spin"
                aria-hidden="true"
              />
            )}
            Save
          </EnhancedButton>
          <DrawerClose asChild>
            <EnhancedButton variant={"outline"} disabled={isPending}>
              Cancel
            </EnhancedButton>
          </DrawerClose>
        </DrawerFooter>
      </DrawerContent>
    </Drawer>
  );
};

export default UpsertAmmoDialog;
