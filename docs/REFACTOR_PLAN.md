# Comprehensive Refactor Plan

## Summary of Requirements
1. Fix map display (white/blank screen)
2. Fix online users display when QR scanned
3. Revert to original login/signup pages in separate routes
4. App opens to MapPage (not auth)
5. Profile page shows login/signup buttons when not authenticated
6. Improve Web Admin CSS
7. Restaurant management: Grid layout (3x2) with QR + info, pagination for 6+ items
8. Sync user data types across all projects

---

## PHASE 1: App Navigation & Auth Flow

### Current Flow (Working):
- App opens → MapPage (correct)
- Profile shows login/signup in VerticalStackLayout (correct)
- After login → profile shows user info (correct)

### Issue to Fix:
- Map appears blank/white on first load
- Need to verify MapPage rendering

### Fix:
1. Add explicit map center initialization in OnAppearing
2. Ensure tile layer loads properly
3. Add error logging for map initialization
4. Test with sample data

---

## PHASE 2: Online Users & QR Scanning

### Current Issues:
- Online users display errors when QR scanned
- No clear error message shown

### Root Cause:
- When QR scanned → PlayHistory record created
- Need to sync online status when scanning QR

### Fixes:
1. Add error handling in QR scan event
2. Update online status when POI is played
3. Add proper logging for debugging
4. Test with sample QR codes

---

## PHASE 3: Web Admin Improvements

### Visual Enhancements:
1. **Improve CSS Styling**
   - Add consistent color scheme
   - Better spacing and typography
   - Responsive design

2. **Restaurant Management Grid**
   - Convert from table to grid (3x2 layout)
   - Each item shows:
     - QR code image (large)
     - Restaurant info below in box:
       - Name
       - Address
       - Status
       - Action buttons
   - Pagination: Show 6 items per page
   - Add smooth transitions

---

## PHASE 4: Data Synchronization

### Models to Sync:
- **User**: Language, IsPaid, PaidAt (✅ done)
- **AudioPOI**: QRCode, CreatedAt, UpdatedAt, OwnerId (✅ done)
- **PlayHistory**: UserId (✅ done)
- **UserPayment**: PaidDate nullable (✅ done)

### Verify All:
1. MAUI models match Web Admin models
2. Database initialization correct
3. Seeding data complete
4. API endpoints match

---

## Implementation Order:
1. ✅ Phase 1: Map display fix
2. ✅ Phase 2: Online users / QR scanning
3. ⏳ Phase 3: Web Admin CSS
4. ⏳ Phase 4: Restaurant grid + pagination

---

## Status Tracking
- [ ] Map displays on startup
- [ ] Pins load with geolocation
- [ ] Online users update when QR scanned
- [ ] Web Admin CSS improved
- [ ] Restaurant grid functional
- [ ] Pagination working
- [ ] All tests passing
