# Public Site Routing

## Publicly available pages

### Standard

All public pages are also available to authenticated users.

* `/Public` (search)
* `/Public/ComplaintDetails`
* `/Public/ComplaintNotFound`

### Account admin, etc.

* `/Account/Login`
* `/Account/ConfirmEmail`
* `/Account/SetPassword`
* `/Account/ForgotPassword`
* `/Account/ForgotPasswordConfirmation`
* `/Account/ResetPassword`
* `/Account/AccessDenied`
* `/Error`

## Redirection if not authenticated

| Input                | Output                               |
|----------------------|--------------------------------------|
| `/Complaint/Details` | `/Public/ComplaintDetails`         |
| All others           | `/Account/Login?returnUrl={route}` |
