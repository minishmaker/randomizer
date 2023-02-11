.thumb

@this is just what vanilla does
ldr	r3,=#0x808009C
mov	lr,r3
mov	r3,#5
.short	0xf800
cmp	r0,#0
bne	noTransition

@check if link is close enough
ldr	r1,=#0x300118C
ldr	r1,[r1]	@link X position, with subpixels
ldr	r3,=#0x3000BF0
ldrh	r2,[r3,#0x06]	@room X coord in area
ldrh	r3,[r3,#0x1E]	@room horizontal size
add	r3,r2	@X coord + size = X coord of the right border
mov	r0,#0x23	@slightly less than the size of a lilypad

@we add/substract the size of the pad to the borders
add	r2,r0
sub	r3,r0
@and we shift them so they are the proper size to be compared with a coord with subpixels
lsl	r2,r2,#16
lsl	r3,r3,#16

@now we check if link is close enough to any of the two horizontal borders
cmp	r1,r2
blo	transition
cmp	r1,r3
bhi	transition
b	noTransition

transition:
ldr	r3,=#0x80857DA
mov	lr,r3
.short	0xF800

noTransition:
ldr	r3,=#0x80857F8
mov	lr,r3
.short	0xF800
