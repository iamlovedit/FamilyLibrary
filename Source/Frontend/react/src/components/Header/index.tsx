"use client";
import {
  Navbar,
  NavbarBrand,
  NavbarContent,
  NavbarItem,
  Link,
  Input,
  DropdownItem,
  DropdownTrigger,
  Dropdown,
  DropdownMenu,
  Avatar,
} from '@nextui-org/react'
import { useState } from 'react'
import { AcmeLogo } from '../Logo'
import { SearchIcon } from '../SearchIcon'
import { useLocation } from 'react-router-dom';
import { useNavigate } from "react-router-dom";

type MenuItem = {
  name: string,
  key: string
}

function Header() {
  const location = useLocation()
  const [activeItem, setActiveItem] = useState(location.pathname);
  const navigate = useNavigate()

  const menuItems: MenuItem[] = [
    {
      name: "主页",
      key: "/"
    },
    {
      name: "节点包",
      key: "/dynamo"
    }, {
      name: "族库",
      key: "/family"
    }
  ]

  const handlePressNavbarItem = (event: any, key: string) => {
    event.preventDefault();
    setActiveItem(key)
    navigate(key)
  }


  return (
    <Navbar isBordered>
      <NavbarContent justify="start">
        <NavbarBrand className="mr-4">
          <AcmeLogo />
          <p className="hidden sm:block font-bold text-inherit">Young</p>
        </NavbarBrand>
      </NavbarContent>
      <NavbarContent className="hidden sm:flex gap-4" justify="center">
        {
          menuItems.map((item: MenuItem) => (
            <NavbarItem isActive={activeItem === item.key}>
              <Link color={activeItem === item.key ? 'primary' : 'foreground'} href={item.key} onClick={(e) => handlePressNavbarItem(e, item.key)}>
                {item.name}
              </Link>
            </NavbarItem>
          ))
        }
      </NavbarContent>
      <NavbarContent as="div" className="items-center" justify="end">
        <Input
          classNames={{
            base: 'max-w-full sm:max-w-[10rem] h-10',
            mainWrapper: 'h-full',
            input: 'text-small',
            inputWrapper:
              'h-full font-normal text-default-500 bg-default-400/20 dark:bg-default-500/20'
          }}
          placeholder="Type to search..."
          size="sm"
          startContent={<SearchIcon size={18} />}
          type="search"
        />
        <Dropdown placement="bottom-end">
          <DropdownTrigger>
            <Avatar
              isBordered
              as="button"
              className="transition-transform"
              color="secondary"
              name="Jason Hughes"
              size="sm"
              src="https://i.pravatar.cc/150?u=a042581f4e29026704d"
            />
          </DropdownTrigger>
          <DropdownMenu aria-label="Profile Actions" variant="flat">
            <DropdownItem key="profile" className="h-14 gap-2">
              <p className="font-semibold">Signed in as</p>
              <p className="font-semibold">zoey@example.com</p>
            </DropdownItem>
            <DropdownItem key="settings">My Settings</DropdownItem>
            <DropdownItem key="team_settings">Team Settings</DropdownItem>
            <DropdownItem key="analytics">Analytics</DropdownItem>
            <DropdownItem key="system">System</DropdownItem>
            <DropdownItem key="configurations">Configurations</DropdownItem>
            <DropdownItem key="help_and_feedback">Help & Feedback</DropdownItem>
            <DropdownItem key="logout" color="danger">
              Log Out
            </DropdownItem>
          </DropdownMenu>
        </Dropdown>
      </NavbarContent>


    </Navbar>
  )
}

export default Header
