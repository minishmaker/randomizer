.thumb
ldrb	r0,[r4,#0x0A]
ldr	r1,table
mov	r2,#12
mul	r0,r2
ldrh	r1,[r1,r0]
mov	r0,r4

ldr	r3,=#0x8004344
mov	lr,r3
.short	0xF800

ldr	r3,=#0x809ACC5
bx	r3

.align
.ltorg
table:
