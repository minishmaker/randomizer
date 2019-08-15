.thumb
ldrb	r4,[r4,#0x0A]
ldr	r0,table
mov	r1,#12
mul	r4,r1
add	r4,r0

@set the flag
ldr	r0,[r4,#4]	@base offset
ldr	r1,[r4,#8]	@flag id to set
ldr	r3,=#0x801D5F4	@vanilla flag set routine
mov	lr,r3
.short	0xF800

@get the item
ldrh	r0,[r4]
ldrh	r1,[r4,#2]
mov	r2,#0

@end
ldr	r3,=#0x80A73F8
mov	lr,r3
.short	0xF800
ldr	r3,=#0x805E208
mov	lr,r3
.short	0xF800
pop	{r4,pc}

.align
.ltorg
table:
