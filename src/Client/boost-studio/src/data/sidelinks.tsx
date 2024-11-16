import {
  IconBarrierBlock,
  IconError404,
  IconExclamationCircle,
  IconLayoutDashboard,
  IconLock,
  IconServerOff,
  IconSettings,
} from "@tabler/icons-react"
import { CiFileOn } from "react-icons/ci"
import { HiOutlineStatusOnline } from "react-icons/hi"
import { HiOutlineWrench } from "react-icons/hi2"

export interface NavLink {
  title: string
  label?: string
  href: string
  icon: JSX.Element
}

export interface SideLink extends NavLink {
  sub?: NavLink[]
}

export const sidelinks: SideLink[] = [
  {
    title: "Units",
    label: "",
    href: "/units",
    icon: <IconLayoutDashboard size={18} />,
  },
  // {
  //   title: "Live",
  //   label: "",
  //   href: "/live",
  //   icon: <HiOutlineStatusOnline size={18} />,
  // },
  {
    title: "Patches",
    label: "",
    href: "/patches",
    icon: <CiFileOn size={18} />,
  },
  {
    title: "Tools",
    label: "",
    href: "/tools",
    icon: <HiOutlineWrench size={18} />,
  },
  // {
  //   title: "Error Pages",
  //   label: "",
  //   href: "",
  //   icon: <IconExclamationCircle size={18} />,
  //   sub: [
  //     {
  //       title: "Not Found",
  //       label: "",
  //       href: "/404",
  //       icon: <IconError404 size={18} />,
  //     },
  //     {
  //       title: "Internal Server Error",
  //       label: "",
  //       href: "/500",
  //       icon: <IconServerOff size={18} />,
  //     },
  //     {
  //       title: "Maintenance Error",
  //       label: "",
  //       href: "/503",
  //       icon: <IconBarrierBlock size={18} />,
  //     },
  //     {
  //       title: "Unauthorised Error",
  //       label: "",
  //       href: "/401",
  //       icon: <IconLock size={18} />,
  //     },
  //   ],
  // },
  {
    title: "Settings",
    label: "",
    href: "/settings",
    icon: <IconSettings size={18} />,
  },
]
