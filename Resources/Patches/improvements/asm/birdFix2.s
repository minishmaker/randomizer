.thumb
ldrb	r2,[r4,#0x09]
cmp	r2,#0x6A
bne	checkbird
ldrb	r1,[r4,#0x0A]
cmp	r1,#0x12
bne	checkbird
ldrb	r1,[r4,#0x0B]
cmp	r1,#0x09
bne	checkbird
ldr	r0,=#0x3000BF0
ldrb	r1,[r0,#4]
cmp	r1,#0x68
bne	checkbird
ldrb	r1,[r0,#5]
cmp	r1,#0
bne	checkbird
ldr	r1,=#0x800DA5B
ldrb	r1,[r1]
strb	r1,[r4,#0x0B]
ldr	r0,=#0x80A2074
ldr	r2,[r0]
ldr	r1,=#0x800DA5A
ldrb	r1,[r1]
mov	r0,r4
ldr	r3,=#0x80A2065
bx	r3

checkbird:
ldrb	r1, [r4,#0x0A]
cmp	r2,#0x95
bne	vanilla
ldrb	r3,[r4,#0x0E]
cmp	r3,#0xFE
blo	vanilla
mov	r2,#0
vanilla:
lsl	r2,#3
ldr	r0,=#0x80A2074
ldr	r0,[r0]
add	r2,r0

@check if this is a trap
push	{r2}
ldrb	r3, [r4,#0x09]
cmp	r3, #0x4D
beq	notTrap
ldrb	r3, [r4, #0x0A]
cmp	r3, #0x1B
bne	notTrap
ldrb	r3, [r4,#0x08]
cmp	r3, #0x06
bne	notTrap
mov	r0, r4
ldr	r3, trapGetIcon
mov	lr, r3
.short	0xF800
mov	r1, r0

notTrap:
pop	{r2}
mov	r0,r4
ldr	r3,=#0x80A2065
bx	r3
.align
.ltorg
trapGetIcon:
