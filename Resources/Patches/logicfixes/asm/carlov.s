.equ sub, item+4
.thumb
push	{r4,lr}
mov	r4,r1
ldr	r0,[r4]
ldr	r3,=#0x80169CE
mov	lr,r3
.short	0xF800
ldr	r3,=#0x807C654
mov	lr,r3
.short	0xF800
str	r0,[r4,#20]

@check if area and room are correct
ldr	r0,=#0x3000BF0
mov	r1,#0x23
ldrb	r2,[r0,#4]
cmp	r2,r1
bne	end
mov	r1,#0x07
ldrb	r2,[r0,#5]
cmp	r2,r1
bne	end

mov	r0,#4
str	r0,[r4,#20]

@check if custom flag is set
ldr	r0,=#0x2002EA4
ldr	r1,=#9
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
bne	end

@if not set, hand out item
ldr	r0,item
ldr	r1,sub
mov	r2,#0
ldr	r3,=#0x80A7410
mov	lr,r3
.short	0xF800
ldr	r0,=#0x2002EA4
mov	r1,#9
ldr	r3,=#0x801D5F4	@vanilla flag set routine
mov	lr,r3
.short	0xF800

end:
ldr	r3,=#0x807DC75
bx	r3

.align
.ltorg
item:
