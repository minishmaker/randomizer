.thumb
push	{r4,lr}
ldr	r4,table
ldrb	r0,[r0,#0x0E]
mov	r2,#3
mul	r0,r2
add	r4,r0

@set the flag
ldr	r0,=#0x2002EA4
ldrb	r1,[r4,#0]
ldr	r3,=#0x801D5F4	@vanilla flag set routine
mov	lr,r3
.short	0xF800

@grant the item
ldrb	r0,[r4,#1]
ldrb	r1,[r4,#2]
mov	r2,#0
ldr	r3,=#0x80A7410
mov	lr,r3
.short	0xF800
pop	{r4,pc}
.align
.ltorg
table:
