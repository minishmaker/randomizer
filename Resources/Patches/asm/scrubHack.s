.equ return, table+4
.thumb
push	{r1}
ldr	r0,table
loop:
ldr	r1,[r0]
cmp	r1,#0
beq	vanilla
cmp	r1,r6
beq	match
add	r0,#8
b	loop

match:
ldrh	r1,[r0,#6]
cmp	r1,#0xFF
beq	replace
pop	{r2}
b	jump
replace:
pop	{r1}
jump:
ldrh	r0,[r0,#4]
b	end

vanilla:
ldrb	r0,[r6,#8]
pop	{r1}

end:
mov	r2,#0
ldr	r3,=#0x80A73F8
mov	lr,r3
.short	0xF800
ldr	r3,return
bx	r3

.align
.ltorg
table:
