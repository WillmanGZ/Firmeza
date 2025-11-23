# Firmeza Client Web - README

## 📋 Table of Contents

- [Project Overview](#project-overview)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Available Scripts](#available-scripts)
- [Architecture](#architecture)
- [Key Components](#key-components)
- [Services](#services)
- [State Management](#state-management)
- [Styling](#styling)
- [Authentication](#authentication)
- [Shopping Cart](#shopping-cart)
- [Notifications](#notifications)
- [Deployment](#deployment)
- [User Stories Implementation](#user-stories-implementation)
- [IDE Setup](#ide-setup)
- [Troubleshooting](#troubleshooting)
- [Browser Support](#browser-support)
- [Contributing](#contributing)

---

## Project Overview

**Firmeza Client Web** is a modern e-commerce web application built with Vue 3 and TypeScript. It serves as the front-end platform for the Firmeza sales system, providing users with an intuitive interface to browse products, manage shopping carts, and complete purchases.

The application is designed with a focus on user experience, accessibility, and performance, offering a seamless shopping experience across different devices.

### Purpose

Firmeza is a **sales platform** that enables customers to:

- Register and authenticate securely
- Browse available products
- Add items to their shopping cart
- Complete purchases
- Receive order confirmations

---

## Features

### 🔐 Authentication

- User registration with email, username, and phone number validation
- Secure login with JWT token-based authentication
- Session persistence using browser cookies
- Automatic logout and session management

### 🛍️ Product Management

- Browse all available products with descriptions and pricing
- Real-time product listing from the backend API
- Product details display with image placeholders
- Easy-to-use product cards with add-to-cart functionality

### 🛒 Shopping Cart

- Add/remove products from cart
- Adjust product quantities
- Real-time cart total calculation
- Persistent cart storage using localStorage
- Clear entire cart functionality

### 💳 Checkout

- One-click checkout process
- Order summary review
- Integrated payment processing through the backend
- Order confirmation notifications

### 🎨 User Interface

- Responsive design that works on mobile, tablet, and desktop
- Clean, modern design with Tailwind CSS
- Intuitive navigation sidebar
- Toast notifications for user feedback
- Accessible form inputs and buttons

---

## Tech Stack

### Frontend Framework

- **Vue 3** - Progressive JavaScript framework for building user interfaces
- **TypeScript** - Strongly-typed JavaScript for better development experience
- **Vue Router** - Official router for Vue.js applications

### Styling & UI

- **Tailwind CSS** - Utility-first CSS framework
- **CSS** - Custom styling with Tailwind integration

### Build Tools

- **Vite** - Next-generation frontend build tool
- **npm** - Package manager

### Libraries & Utilities

- **js-cookie** - Simple cookie management library
- **SweetAlert2** - Beautiful notification alerts
- **@tailwindcss/vite** - Tailwind CSS integration for Vite

### Development Tools

- **ESLint** - Code quality and style enforcement
- **Prettier** - Code formatter
- **TypeScript** - Static type checking

### Deployment

- **Vercel** - Hosting platform with automatic deployments

---

## Project Structure

```
Client.Web/
├── src/
│   ├── assets/
│   │   └── icons/                 # SVG icon components
│   │       ├── HomeIcon.vue
│   │       ├── CartIcon.vue
│   │       ├── BookIcon.vue
│   │       ├── MailIcon.vue
│   │       ├── LockIcon.vue
│   │       ├── UserIcon.vue
│   │       └── LogOutIcon.vue
│   │
│   ├── components/
│   │   └── AsideBar.vue           # Main navigation sidebar
│   │
│   ├── composables/
│   │   ├── useAuth.ts             # Authentication state & methods
│   │   └── useCart.ts             # Shopping cart state & methods
│   │
│   ├── helpers/
│   │   └── handle-response.ts     # API response handling utility
│   │
│   ├── interfaces/
│   │   ├── api-response.ts        # Generic API response type
│   │   ├── product.ts             # Product data type
│   │   ├── user-login.ts          # Login request type
│   │   ├── user-register.ts       # Registration request type
│   │   ├── sale-full-create.ts    # Order creation type
│   │   ├── sale-full-product.ts   # Order item type
│   │   └── sale-full-response.ts  # Order response type
│   │
│   ├── layouts/
│   │   └── ShopLayout.vue         # Main layout wrapper
│   │
│   ├── router/
│   │   └── router.ts              # Vue Router configuration
│   │
│   ├── services/
│   │   ├── auth.service.ts        # Authentication API calls
│   │   ├── products.service.ts    # Products API calls
│   │   └── checkout.service.ts    # Checkout/Orders API calls
│   │
│   ├── utils/
│   │   └── ToastService.ts        # Notification system
│   │
│   ├── views/
│   │   ├── LoginView.vue          # Login page
│   │   ├── RegisterView.vue       # Registration page
│   │   ├── HomeView.vue           # Home/Dashboard page
│   │   ├── ProductsView.vue       # Products listing page
│   │   └── CartView.vue           # Shopping cart page
│   │
│   ├── App.vue                    # Root component
│   ├── main.ts                    # Application entry point
│   └── styles.css                 # Global styles
│
├── public/                        # Static assets
├── index.html                     # HTML entry point
├── vite.config.ts                 # Vite configuration
├── tailwind.config.ts             # Tailwind CSS configuration
├── tsconfig.json                  # TypeScript configuration
├── eslint.config.ts               # ESLint configuration
└── package.json                   # Project dependencies
```

---

## Getting Started

### Prerequisites

- Node.js (v20.19.0 or >=22.12.0)
- npm (v8 or higher)
- Git

### Installation

1. **Clone the repository:**

```bash
git clone https://github.com/WillmanGZ/firmeza.git
cd Client.Web
```

2. **Install dependencies:**

```bash
npm install
```

3. **Set up environment variables:**
   Create a `.env` file in the root directory:

```env
VITE_API_URL=http://localhost:5152/api
```

4. **Start the development server:**

```bash
npm run dev
```

The application will be available at `http://localhost:5173`

---

## Available Scripts

### Development

```bash
npm run dev
```

Starts the development server with hot module replacement (HMR).

### Build

```bash
npm run build
```

Compiles the project with type checking and builds optimized production bundles.

### Preview

```bash
npm run preview
```

Previews the production build locally.

### Type Checking

```bash
npm run type-check
```

Runs TypeScript compiler to check for type errors.

### Linting

```bash
npm run lint
```

Runs ESLint and automatically fixes code style issues.

---

## Architecture

### Component-Based Architecture

The application follows Vue 3's composition API pattern using composables for shared logic:

```
Views (Pages)
    ├── LoginView
    ├── RegisterView
    ├── HomeView
    ├── ProductsView
    └── CartView
         │
         ├─→ Composables (State)
         │    ├── useAuth()
         │    └── useCart()
         │
         ├─→ Services (API)
         │    ├── authService
         │    ├── productService
         │    └── checkoutService
         │
         └─→ Components
              └── AsideBar
```

### Data Flow

**Authentication Flow:**

```
LoginView → authService.login() → API → useAuth.setUserInfo() → Redirect to Home
```

**Shopping Flow:**

```
ProductsView → ProductCard → useCart.add() → localStorage → CartView
```

**Checkout Flow:**

```
CartView → checkoutService.process() → API → Toast Notification → Clear Cart
```

---

## Key Components

### 📄 Views

#### **LoginView.vue**

- User login form with email and password
- Form validation
- Error handling with toast notifications
- Redirect to dashboard on successful login

#### **RegisterView.vue**

- User registration form
- Fields: username, email, phone number, password
- Input validation
- Success message with redirect to login

#### **HomeView.vue**

- Welcome screen with platform overview
- Feature highlights (quality products, easy experience, fast cart)
- Call-to-action button to browse products

#### **ProductsView.vue**

- Displays grid of all available products
- Loading state handling
- Product cards with:
  - Product name and description
  - Price display
  - Add to cart button
- Empty state handling

#### **CartView.vue**

- List of items in the shopping cart
- Quantity adjustment controls (+ / -)
- Remove item functionality
- Real-time total calculation
- Checkout button
- Empty cart message

### 🧩 Components

#### **AsideBar.vue**

- Navigation menu with:
  - Home link
  - Products link
  - Cart link
  - Logout button
- Mobile-responsive with toggle menu
- Active route indicator
- Firmeza branding

---

## Services

### 🔐 Authentication Service (`src/services/auth.service.ts`)

```typescript
authService.login(user: UserLogin): Promise<ApiResponse<string>>
authService.register(user: UserRegister): Promise<ApiResponse<...>>
```

Handles:

- User login with credentials
- User registration
- Token management

### 📦 Products Service (`src/services/products.service.ts`)

```typescript
productService.getAll(): Promise<ApiResponse<Product[]>>
productService.getById(id: string): Promise<ApiResponse<Product>>
```

Handles:

- Fetching all products
- Fetching individual product details

### 💳 Checkout Service (`src/services/checkout.service.ts`)

```typescript
checkoutService.process(items: CartItem[]): Promise<ApiResponse<string>>
```

Handles:

- Processing orders
- Sending cart items to backend
- Order confirmation

---

## State Management

### 🔐 Authentication (`src/composables/useAuth.ts`)

**Composable for managing user authentication state:**

```typescript
const {
  userToken,           // Current user's JWT token
  setUserInfo(),       // Store token in cookies
  getToken(),          // Retrieve token
  removeUserInfo(),    // Clear token (logout)
  isAuthenticated()    // Check if user is logged in
} = useAuth()
```

**Storage:** Browser cookies with 7-day expiration

### 🛒 Shopping Cart (`src/composables/useCart.ts`)

**Composable for managing shopping cart state:**

```typescript
const {
  items,               // Array of CartItem[]
  total,               // Computed total price
  add(product),        // Add product to cart
  remove(id),          // Remove item from cart
  decrease(id),        // Reduce quantity or remove
  increase(id),        // Increase quantity
  clear()              // Empty cart
} = useCart()
```

**Storage:** localStorage with `firmeza_cart` key

---

## Styling

### Tailwind CSS

The project uses Tailwind CSS for utility-first styling:

- **Color Scheme:** Blue (#1E40AF) as primary, with grays and accent colors
- **Responsive Design:** Mobile-first approach with breakpoints
- **Components:** Styled buttons, cards, forms, and navigation
- **Icons:** SVG-based custom icons

### Global Styles (`src/styles.css`)

```css
@import 'tailwindcss';

* {
  @apply font-sans;
}
```

---

## Authentication

### Authentication Flow

1. **Registration:**
   - User submits registration form
   - Backend creates user account
   - User redirected to login page

2. **Login:**
   - User submits credentials
   - Backend validates and returns JWT token
   - Token stored in cookies (7-day expiration)
   - User redirected to dashboard

3. **Authorization:**
   - Protected routes require valid token
   - Router guards check authentication status
   - Unauthenticated users redirected to login

4. **Session Persistence:**
   - Token stored in cookies
   - Loaded on app initialization
   - Automatic logout on expiration

### Route Protection

Routes are protected with guards that verify authentication status before allowing access.

---

## Shopping Cart

### Cart Features

1. **Add to Cart:**
   - Click "Add to Cart" on product card
   - Product added with quantity 1
   - Toast notification confirms action

2. **Manage Cart:**
   - Increase/decrease quantities
   - Remove individual items
   - Clear entire cart

3. **Persistence:**
   - Cart saved to localStorage
   - Survives page refreshes
   - Synced with user session

4. **Checkout:**
   - Review items and total
   - Submit order with one click
   - Order confirmation notification
   - Cart cleared after successful order

### Cart Data Structure

```typescript
interface CartItem {
  id: string; // Unique cart item ID
  product: Product; // Product details
  quantity: number; // Item quantity
}
```

---

## Notifications

### Toast Service (`src/utils/ToastService.ts`)

Uses SweetAlert2 for elegant toast notifications:

```typescript
ToastService.success(message); // Green success toast
ToastService.info(message); // Blue info toast
ToastService.warning(message); // Yellow warning toast
ToastService.error(message); // Red error toast
```

**Features:**

- Auto-dismiss after 3 seconds
- Pause on hover
- Top-right corner positioning
- Progress bar indicator

---

## Deployment

### Vercel Configuration (`vercel.json`)

```json
{
  "rewrites": [{ "source": "/(.*)", "destination": "/" }]
}
```

This configuration ensures that all routes are rewritten to `index.html` for client-side routing.

### Deployment Steps

1. **Build the project:**

```bash
npm run build
```

2. **Deploy to Vercel:**

```bash
vercel
```

Or connect your GitHub repository to Vercel for automatic deployments on push.

### Environment Variables (Vercel)

Set in Vercel dashboard:

```
VITE_API_URL=https://api.example.com
```

---

## User Stories Implementation

### 📋 User Story: Registration

**As a** new user  
**I want to** create an account with my credentials  
**So that** I can access the Firmeza platform

**Implementation:** `src/views/RegisterView.vue` with form validation and API integration

**Acceptance Criteria:**

- User can enter username, email, phone, and password
- Form validates input before submission
- Success message displays on successful registration
- User is redirected to login page

---

### 📋 User Story: Login

**As a** registered user  
**I want to** log in with my credentials  
**So that** I can access my account

**Implementation:** `src/views/LoginView.vue` with JWT token storage in cookies

**Acceptance Criteria:**

- User can enter email and password
- Valid credentials return JWT token
- Token is stored in cookies
- User is redirected to home page
- Failed login shows error message

---

### 📋 User Story: Browse Products

**As a** logged-in user  
**I want to** view available products  
**So that** I can decide what to purchase

**Implementation:** `src/views/ProductsView.vue` fetching from products API

**Acceptance Criteria:**

- All products are displayed in a grid
- Each product shows name, description, and price
- Loading state is displayed while fetching
- Empty state is shown if no products exist

---

### 📋 User Story: Shopping Cart

**As a** user  
**I want to** add products to my cart and adjust quantities  
**So that** I can manage my purchases

**Implementation:** `src/composables/useCart.ts` with localStorage persistence

**Acceptance Criteria:**

- User can add products from product view
- Quantity can be increased/decreased
- Items can be removed from cart
- Cart persists across page refreshes
- Total is calculated correctly

---

### 📋 User Story: Checkout

**As a** user  
**I want to** complete my purchase  
**So that** I can receive my order confirmation

**Implementation:** `src/views/CartView.vue` with checkout service integration

**Acceptance Criteria:**

- Cart items are displayed before checkout
- Total price is calculated and displayed
- User can proceed to checkout
- Order confirmation is shown
- Cart is cleared after successful order

---

## IDE Setup

### Recommended IDE Setup

[VS Code](https://code.visualstudio.com/) + [Vue (Official)](https://marketplace.visualstudio.com/items?itemName=Vue.volar) (and disable Vetur).

### Recommended Extensions

- **Volar** - Official Vue 3 support for VS Code
- **TypeScript Vue Plugin** - TypeScript support for Vue files
- **ESLint** - Code quality
- **Prettier** - Code formatter
- **Tailwind CSS IntelliSense** - Tailwind CSS support

### Recommended Browser Setup

- Chromium-based browsers (Chrome, Edge, Brave, etc.):
  - [Vue.js devtools](https://chromewebstore.google.com/detail/vuejs-devtools/nhdogjmejiglipccpnnnanhbledajbpd)
  - [Turn on Custom Object Formatter in Chrome DevTools](http://bit.ly/object-formatters)
- Firefox:
  - [Vue.js devtools](https://addons.mozilla.org/en-US/firefox/addon/vue-js-devtools/)
  - [Turn on Custom Object Formatter in Firefox DevTools](https://fxdx.dev/firefox-devtools-custom-object-formatters/)

---

## Type Support for `.vue` Imports in TS

TypeScript cannot handle type information for `.vue` imports by default, so we replace the `tsc` CLI with `vue-tsc` for type checking. In editors, we need [Volar](https://marketplace.visualstudio.com/items?itemName=Vue.volar) to make the TypeScript language service aware of `.vue` types.

---

## Troubleshooting

### Port Already in Use

```bash
# Use a different port
npm run dev -- --port 3000
```

### Build Errors

```bash
# Clear node_modules and reinstall
rm -r node_modules
npm install
npm run build
```

### Authentication Issues

- Check if backend API is running
- Verify `VITE_API_URL` is correct in `.env`
- Clear browser cookies and localStorage

### Cart Not Persisting

- Check if localStorage is enabled
- Verify browser storage hasn't reached quota
- Check browser console for errors

### HMR Not Working

- Ensure your firewall isn't blocking the HMR port
- Check that `vite.config.ts` is properly configured
- Try clearing `.vite` cache directory

---

## Browser Support

- Chrome/Edge (latest 2 versions)
- Firefox (latest 2 versions)
- Safari (latest 2 versions)
- Mobile browsers (iOS Safari, Chrome Mobile)

---

## API Endpoints

The application communicates with the backend API at `http://localhost:5152/api`:

| Method | Endpoint         | Purpose           |
| ------ | ---------------- | ----------------- |
| POST   | `/auth/login`    | User login        |
| POST   | `/auth/register` | User registration |
| GET    | `/products`      | Get all products  |
| GET    | `/products/{id}` | Get product by ID |
| POST   | `/sales/full`    | Create order      |

---

## Performance Optimization

- **Code Splitting:** Route-based code splitting with Vue Router
- **Lazy Loading:** Images and components loaded on demand
- **Caching:** API responses cached where appropriate
- **Build Optimization:** Vite's optimized build process

---

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit changes (`git commit -m 'Add amazing feature'`)
4. Push to branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Code Style

- Follow ESLint rules
- Format with Prettier
- Write TypeScript-first code
- Add comments for complex logic

---

## License

This project is licensed under the MIT License.

---

## Support & Contact

For issues, questions, or feedback:

- Create an issue in the GitHub repository
- Contact the development team

---

## Changelog

### Version 1.0.0

- Initial release
- Authentication system
- Product browsing
- Shopping cart
- Checkout functionality
- Responsive design

---

**Last Updated:** November 2025  
**Maintainer:** Firmeza Development Team
