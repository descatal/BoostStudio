import React from "react"
import { exportAmmoByPath } from "@/api/wrapper/ammo-api"
import { exportHitboxesByPath } from "@/api/wrapper/hitbox-api"
import { exportProjectilesByPath } from "@/api/wrapper/projectile-api"
import { exportStatsByPath } from "@/api/wrapper/stats-api"
import { useExportDialogStore } from "@/pages/units/customize/information/components/export-dialog/libs/store"
import { useUnitsStore } from "@/pages/units/libs/store"
import { zodResolver } from "@hookform/resolvers/zod"
import { ReloadIcon } from "@radix-ui/react-icons"
import { useForm } from "react-hook-form"

import { Button } from "@/components/ui/button"
import { Checkbox } from "@/components/ui/checkbox"
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog"
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
} from "@/components/ui/form"
import { Sheet } from "@/components/ui/sheet"
import { toast } from "@/components/ui/use-toast"

import { exportDialogSchema, ExportDialogSchema } from "./libs/validations"

interface ExportDialogProps extends React.ComponentPropsWithRef<typeof Sheet> {}

const ExportDialog = ({ ...props }: ExportDialogProps) => {
  const {
    openExportDialog,
    statsSelected,
    ammoSelected,
    projectilesSelected,
    hitboxesSelected,
    setOpenExportDialog,
    setStatsSelected,
    setAmmoSelected,
    setHitboxesSelected,
    setProjectilesSelected,
  } = useExportDialogStore()
  const unit = useUnitsStore((state) => state.selectedUnit)
  const [isCreatePending, startCreateTransition] = React.useTransition()

  const form = useForm<ExportDialogSchema>({
    resolver: zodResolver(exportDialogSchema),
    defaultValues: {
      unitId: unit?.unitId,
      stats: statsSelected,
      ammo: ammoSelected,
      projectile: projectilesSelected,
      hitbox: hitboxesSelected,
    },
  })

  function onSubmit(input: ExportDialogSchema) {
    startCreateTransition(async () => {
      setStatsSelected(input.stats ?? false)
      setAmmoSelected(input.ammo ?? false)
      setHitboxesSelected(input.hitbox ?? false)
      setProjectilesSelected(input.projectile ?? false)

      if (!input.unitId) return

      if (input.ammo) {
        await exportAmmoByPath({})
      }

      if (input.stats) {
        await exportStatsByPath({
          unitIds: [input.unitId],
        })
      }

      if (input.projectile) {
        await exportProjectilesByPath({
          unitIds: [input.unitId],
        })
      }

      if (input.hitbox) {
        await exportHitboxesByPath({
          unitIds: [input.unitId],
        })
      }

      // form.reset()
      props.onOpenChange?.(false)
      setOpenExportDialog(false)
      toast({
        title: `Export operation complete!`,
      })
    })
  }

  return (
    <Dialog open={openExportDialog} onOpenChange={setOpenExportDialog}>
      <DialogContent>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)}>
            <DialogHeader>
              <DialogTitle>Export Data</DialogTitle>
              <DialogDescription>
                Export data to your working directory.
              </DialogDescription>
            </DialogHeader>
            <div className="flex flex-col gap-4 py-4">
              <FormField
                control={form.control}
                name="stats"
                render={({ field }) => (
                  <FormItem className="flex flex-row items-start space-x-3 space-y-0 rounded-md border p-4 shadow">
                    <FormControl>
                      <Checkbox
                        checked={field.value}
                        onCheckedChange={field.onChange}
                      />
                    </FormControl>
                    <div className="space-y-1 leading-none">
                      <FormLabel>Stats</FormLabel>
                      <FormDescription>
                        Export stats data for this unit.
                      </FormDescription>
                    </div>
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="ammo"
                render={({ field }) => (
                  <FormItem className="flex flex-row items-start space-x-3 space-y-0 rounded-md border p-4 shadow">
                    <FormControl>
                      <Checkbox
                        checked={field.value}
                        onCheckedChange={field.onChange}
                      />
                    </FormControl>
                    <div className="space-y-1 leading-none">
                      <FormLabel>Ammo</FormLabel>
                      <FormDescription>
                        Export ammo data for ALL units.
                      </FormDescription>
                    </div>
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="projectile"
                render={({ field }) => (
                  <FormItem className="flex flex-row items-start space-x-3 space-y-0 rounded-md border p-4 shadow">
                    <FormControl>
                      <Checkbox
                        checked={field.value}
                        onCheckedChange={field.onChange}
                      />
                    </FormControl>
                    <div className="space-y-1 leading-none">
                      <FormLabel>Projectile</FormLabel>
                      <FormDescription>
                        Export projectiles data for this unit.
                      </FormDescription>
                    </div>
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="hitbox"
                render={({ field }) => (
                  <FormItem className="flex flex-row items-start space-x-3 space-y-0 rounded-md border p-4 shadow">
                    <FormControl>
                      <Checkbox
                        checked={field.value}
                        onCheckedChange={field.onChange}
                      />
                    </FormControl>
                    <div className="space-y-1 leading-none">
                      <FormLabel>Hitbox</FormLabel>
                      <FormDescription>
                        Export hitbox data for this unit.
                      </FormDescription>
                    </div>
                  </FormItem>
                )}
              />
            </div>
            <DialogFooter>
              <Button disabled={isCreatePending} type={"submit"}>
                {isCreatePending && (
                  <ReloadIcon
                    className="size-4 mr-2 animate-spin"
                    aria-hidden="true"
                  />
                )}
                Save
              </Button>
            </DialogFooter>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  )
}

export default ExportDialog
