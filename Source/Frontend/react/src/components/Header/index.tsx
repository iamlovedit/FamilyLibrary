'use client'
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
  Button
} from '@nextui-org/react'
import { useState } from 'react'
import { useTheme } from 'next-themes'
import { useLocation, useNavigate } from 'react-router-dom'
import { AcmeLogo } from '@/components/Icons/Logo'
import { SunIcon } from '@/components/Icons/Sun'
import { Moon } from '@/components/Icons/Moon'

type MenuItem = {
  name: string
  key: string
}

function Header() {
  const location = useLocation()
  const { theme, setTheme } = useTheme()
  const [activeItem, setActiveItem] = useState(location.pathname)
  const navigate = useNavigate()

  const handleThemeChange = () => {
    if (theme === 'light') {
      setTheme('dark')
    } else {
      setTheme('light')
    }
  }

  const menuItems: MenuItem[] = [
    {
      name: '主页',
      key: '/'
    },
    {
      name: '节点包',
      key: '/dynamo'
    },
    {
      name: '族库',
      key: '/family'
    }
  ]

  const handlePressNavbarItem = (event: any, key: string) => {
    event.preventDefault()
    setActiveItem(key)
    navigate(key)
  }

  return (
    <Navbar isBordered maxWidth={'full'}>
      <NavbarContent justify="start">
        <NavbarBrand className="mr-4">
          <AcmeLogo />
          <p className="hidden sm:block font-bold text-inherit">Young</p>
        </NavbarBrand>
      </NavbarContent>
      <NavbarContent className="hidden sm:flex gap-4" justify="center">
        {menuItems.map((item: MenuItem) => (
          <NavbarItem isActive={activeItem === item.key} key={item.key}>
            <Link
              color={activeItem === item.key ? 'primary' : 'foreground'}
              href={item.key}
              onClick={(e) => handlePressNavbarItem(e, item.key)}
            >
              {item.name}
            </Link>
          </NavbarItem>
        ))}
      </NavbarContent>
      <NavbarContent as="div" className="items-center" justify="end">
        <Button isIconOnly variant="light" onClick={handleThemeChange}>
          {theme === 'light' ? <Moon /> : <SunIcon />}
        </Button>
      </NavbarContent>
    </Navbar>
  )
}

export default Header
